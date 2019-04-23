/* Photo upload section */
import React, { Component } from 'react';
import Cookies from 'js-cookie';
import { Icon, Grid, IconGroup, Image, Button } from 'semantic-ui-react';

export default class PhotoUpload extends Component {

    constructor(props) {
        super(props);
        console.log(props)
        const imageId = props.imageId ? props.imageId
            : ""
        const imgSource = props.imgSource ? props.imgSource : ""
        this.state = {
            changeImage: false,
            imageId: imageId,
            fileImg: '',
            imgSource: imgSource
        }
        this.displayImg = this.displayImg.bind(this)
        this.changeImg = this.changeImg.bind(this)
        this.loadImage = this.loadImage.bind(this)
        this.updateImg = this.updateImg.bind(this)
        this.pickedPhoto = this.pickedPhoto.bind(this)
        this.getObjectURL = this.getObjectURL.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        console.log(nextProps)
        this.setState({
            imageId: nextProps.imageId,
            imgSource: nextProps.imgSource,
        });
    }

    changeImg(event) {
        event.persist();
        console.log(event)
        this.setState({
            fileImg: event.target.files[0],
            changeImage: true,
            imageId: event.target.files[0].name
        })
        //console.log('how to pick');
    }

    loadImage() {
        var cookies = Cookies.get('talentAuthToken');
        console.log(this.state.imageId)
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getProfileImage?Id='+this.state.imageId,
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ imgSource: res.profilePath.result })
                this.props.updateProfileData({ profilePhoto: this.state.imageId, profilePhotoUrl:this.state.imgSource})
            }.bind(this)
        })
    }

    updateImg() {
        let data = new FormData();
        data.append('file', this.state.fileImg);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateProfilePhoto',
            headers: {
                'Authorization': 'Bearer ' + cookies,

            },
            type: "POST",
            data: data,
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                //console.log(res)
                if (res.success == true) {
                    TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                    this.setState({ changeImage: false })
                    this.loadImage();
                } else {
                    TalentUtil.notification.show("Profile did not update successfully", "error", null, null)
                }
            }.bind(this),
            error: function (res, a, b) {
                console.log(res)
                //console.log(a)
                //console.log(b)
            }
        })

    }

    displayImg() {
        return (
            <Grid.Column>
                <Image src={this.state.imgSource} size='medium' circular onClick={() => this.refs.refImgChanged.click()} />
                <input ref="refImgChanged" hidden type='file' onChange={this.changeImg} />
            </Grid.Column>)
    }

    getObjectURL(file) {
        
        let url = null;
        if (window.createObjectURL !== undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL !== undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL !== undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }

    //clickimg() {
    //    return alert("ok")
    //}

    pickedPhoto() {
        return (
            <Grid.Column>
                <div className="field" >
                    <Image src={this.getObjectURL(this.state.fileImg)} size='medium' circular onClick={() => this.refs.refImgChange.click()} />
                    <input ref="refImgChange" hidden type='file' onChange={this.changeImg} />
                </div>
                <div className="field" >
                    <Button type='button' color='black' onClick={this.updateImg}>
                        <Icon name='upload' />
                        Upload
                </Button>
                </div>
            </Grid.Column>
        )
    }

    render() {
        const { imageId, changeImage } = this.state;
        if (changeImage) {
            return this.pickedPhoto();
        }
        else return (
            <Grid centered>
                {imageId ?
                    this.displayImg()
                    :
                    <Grid.Column>
                        <Icon name='camera retro' size='massive' onClick={() => this.refs.refImg.click()}></Icon>
                        <input ref="refImg" hidden type='file' onChange={this.changeImg} />
                    </Grid.Column>
                }
            </Grid>

        )

    }
}
