using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Talent.Common.Aws
{
    public class AwsService : IAwsService
    {
        private AwsOptions _options;
        private SessionAWSCredentials _sessionCredential;
        private IAmazonS3 client;
        public AwsService(IOptions<AwsOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<S3Object>> GetAllObjectFromS3(string bucketName)
        {
            try
            {
                SessionAWSCredentials tempCredentials = _sessionCredential;
                // Create client by providing temporary security credentials.
                using (client = new AmazonS3Client(tempCredentials, Amazon.RegionEndpoint.APSoutheast2))
                {
                    ListObjectsRequest listObjectRequest =
                        new ListObjectsRequest { BucketName = bucketName };
                    // Send request to Amazon S3.
                    ListObjectsResponse response = await client.ListObjectsAsync(listObjectRequest);
                    List<S3Object> objects = response.S3Objects;
                    return objects;
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                    s3Exception.InnerException);
            }
            catch (AmazonSecurityTokenServiceException stsException)
            {
                Console.WriteLine(stsException.Message,
                    stsException.InnerException);
            }
            throw new Exception("Can not list object from Amazone S3 Bucket");
        }
        private async Task<SessionAWSCredentials> GetTemporaryCredentials()
        {
            AmazonSecurityTokenServiceClient stsClient =
    new AmazonSecurityTokenServiceClient(_options.AwsAccessKey,
        _options.AwsSerectKey);
            GetSessionTokenRequest getSessionTokenRequest = new GetSessionTokenRequest { DurationSeconds = 7200 };
            GetSessionTokenResponse sessionTokenResponse = await
                stsClient.GetSessionTokenAsync(getSessionTokenRequest);
            Credentials credentials = sessionTokenResponse.Credentials;

            SessionAWSCredentials sessionCredential =
                new SessionAWSCredentials(credentials.AccessKeyId,
                    credentials.SecretAccessKey,
                    credentials.SessionToken);
            return sessionCredential;
        }

        public async Task<S3Object> GetObjectFromName(string name, string bucketName)
        {
            try
            {
                Console.WriteLine("Listing objects stored in a bucket");
                SessionAWSCredentials tempCredentials = await GetTemporaryCredentials();
                string responseBody = "";
                // Create client by providing temporary security credentials.
                using (client = new AmazonS3Client(tempCredentials, Amazon.RegionEndpoint.APSoutheast2))
                {
                    ListObjectsRequest listObjectRequest =
                        new ListObjectsRequest { BucketName = bucketName };
                    // Send request to Amazon S3.
                    GetObjectRequest getObjectRequest = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = name
                    };
                    using (GetObjectResponse response = await client.GetObjectAsync(getObjectRequest))
                    using (MemoryStream responseStream = new MemoryStream())
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        response.ResponseStream.CopyTo(responseStream);
                        responseBody = reader.ReadToEnd();
                    }
                    return null;
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                    s3Exception.InnerException);
            }
            catch (AmazonSecurityTokenServiceException stsException)
            {
                Console.WriteLine(stsException.Message,
                    stsException.InnerException);
            }
            throw new Exception("Can not list object from Amazone S3 Bucket");
        }

        public async Task<string> GetPresignedUrlObject(string name, string bucketName)
        {
            SessionAWSCredentials tempCredentials = await GetTemporaryCredentials();
            string urlString = "";
            GetPreSignedUrlRequest presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = name,
                Expires = DateTime.Now.AddHours(1)
            };
            try
            {
                using (client = new AmazonS3Client(tempCredentials, Amazon.RegionEndpoint.APSoutheast2))
                {
                    urlString = client.GetPreSignedURL(presignedRequest);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                }
                else
                {

                }
            }
            //catch (Exception e)
            //{

            //}
            return urlString;
        }
        
        //public async Task<string> GetStaticUrl(string name, string bucketName)
        //{
        //    return string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, name);
        //}

        public string GetStaticUrl(string name, string bucketName)
        {
            return string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, name);
        }

        public async Task<bool> PutFileToS3(string name, Stream stream, string bucketName, bool isPublic=false)
        {
            PutObjectRequest s3PutRequest = new PutObjectRequest();
            s3PutRequest = new PutObjectRequest
            {
                InputStream = stream,
                BucketName = bucketName
            };
            if (isPublic)
            {
                s3PutRequest.CannedACL = S3CannedACL.PublicRead;
            }

            //key - new file name
            if (!string.IsNullOrWhiteSpace(name))
            {
                s3PutRequest.Key = name;
            }
            s3PutRequest.Headers.Expires = DateTime.Now.AddHours(24);
            try
            {
                using (client = new AmazonS3Client(await GetTemporaryCredentials(), RegionEndpoint.APSoutheast2))
                {
                    PutObjectResponse s3PutResponse = await client.PutObjectAsync(s3PutRequest);
                    if (s3PutResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveFileFromS3(string name, string bucketName)
        {
            DeleteObjectRequest deleteRequest = new DeleteObjectRequest
            {
                Key = name,
                BucketName = bucketName
            };

            try
            {
                using(client = new AmazonS3Client(await GetTemporaryCredentials(), RegionEndpoint.APSoutheast2))
                {
                    DeleteObjectResponse response = await client.DeleteObjectAsync(deleteRequest);
                    if(response.HttpStatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                    return false;
                }
            }catch(AmazonS3Exception e)
            {
                return false;
            }
        }
    }
}
