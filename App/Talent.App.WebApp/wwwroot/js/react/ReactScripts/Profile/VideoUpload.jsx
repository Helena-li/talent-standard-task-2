import React from 'react'
import Cookies from 'js-cookie'
import { error } from 'util';
import { Progress } from 'semantic-ui-react'
import { Icon, Grid, IconGroup, Image, Button } from 'semantic-ui-react';


export default class VideoUpload extends React.Component {
    constructor(props) {
        super(props)
        const videoName = props.videoName ? props.videoName : ''
        const videoUrl = props.videoUrl ? props.videoUrl : ''
        this.maxLength = 100 * 1024 * 1024; // 100MB - arbitary choice
        this.fileTypes = ['video/mp4']
        this.state = {
            changeNew: false,
            videoName: videoName,
            videoUrl: videoUrl,
            fileInstance: '',
        }
        this.displayVideo = this.displayVideo.bind(this)
        this.changeVideo = this.changeVideo.bind(this)
        this.loadVideo = this.loadVideo.bind(this)
        this.updateVideo = this.updateVideo.bind(this)
        this.pickedVideo = this.pickedVideo.bind(this)
        this.getObjectURL = this.getObjectURL.bind(this)
    }

    loadVideo() {
        var cookies = Cookies.get('talentAuthToken');
        console.log(this.state.videoName)
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getProfileVideo?Id=' + this.state.videoName,
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ videoUrl: res.profilePath.result })
                this.props.updateProfileData({ videoName: this.state.videoName, videoUrl: this.state.videoUrl })
            }.bind(this)
        })
    }

    updateVideo() {
        let data = new FormData();
        data.append('file', this.state.fileInstance);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateTalentVideo',
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
                    this.setState({ changeNew: false })
                    this.loadVideo();
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

    componentWillReceiveProps(nextProps) {
        console.log(nextProps)
        this.setState({
            videoName: nextProps.videoName,
            videoUrl: nextProps.videoUrl,
        });
    }

    changeVideo(event) {
        event.persist();
        console.log(event)
        this.setState({
            fileInstance: event.target.files[0],
            changeNew: true,
            videoName: event.target.files[0].name
        })
        //console.log('how to pick');
    }

    displayVideo() {
        return (
            <Grid.Column>
                <video src={this.state.videoUrl} size='medium' ref='vid' type="video/mp4" height="400"/>
                <Grid.Row>
                <Button type='button' color='black' onClick={() => this.refs.refImgChanged.click()}>
                    <Icon name='pencil' />
                    Edit
                </Button>
                <input ref="refImgChanged" hidden type='file' onChange={this.changeVideo} />
                <Button type='button' color='black' onClick={() => this.refs.vid.play()}>
                   Play
                </Button>
                <Button type='button' color='black' onClick={() => this.refs.vid.pause()}>
                    Pause
                </Button>
                </Grid.Row>
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

    pickedVideo() {
        return (
            <Grid.Column>
                <div className="field" >
                    <video src={this.getObjectURL(this.state.fileInstance)} size='medium' />
                    <Button type='button' color='black' onClick={() => this.refs.refImgChange.click()}>
                        <Icon name='add' />
                        Add
                    </Button>
                    <input ref="refImgChange" hidden type='file' onChange={this.changeVideo} />
                </div>
                <div className="field" >
                    <Button type='button' color='black' onClick={this.updateVideo}>
                        <Icon name='upload' />
                        Upload
                </Button>
                </div>
            </Grid.Column>
        )
    }

    render() {
        const { videoName, changeNew } = this.state;
        if (changeNew) {
            return this.pickedVideo();
        }
        else return (
            <Grid centered>
                {videoName ?
                    this.displayVideo()
                    :
                    <Grid.Column>
                        <Icon name='camera retro' size='massive' onClick={() => this.refs.refImg.click()}></Icon>
                        <input ref="refImg" hidden type='file' onChange={this.changeVideo} />
                    </Grid.Column>
                }
            </Grid>

        )
    }
}