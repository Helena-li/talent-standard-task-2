/* Social media JSX */
import React, { Component } from "react";
import Cookies from 'js-cookie';
import { ChildSingleInput } from '../Form/SingleInput.jsx';
import { Popup, Button, Icon, Grid } from 'semantic-ui-react';

export default class SocialMediaLinkedAccount extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            editDisplay: false,
            newlinkedAccounts: {
                linkedIn: "",
                github: ""
            }
        }
        this.handleClickEdit = this.handleClickEdit.bind(this);
        this.renderEdit = this.renderEdit.bind(this);
        this.renderDisplay = this.renderDisplay.bind(this);
        this.closeEdit = this.closeEdit.bind(this);
        this.savelinkedAccounts = this.savelinkedAccounts.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.goToLinkedin = this.goToLinkedin.bind(this);
        this.goToGithub = this.goToGithub.bind(this);
    }

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.linkedAccounts)
        this.setState({ newlinkedAccounts: nextProps.linkedAccounts });
    }

    componentDidMount() {
        $('.ui.button.social-media').popup();
    }

    handleClickEdit() {
        this.setState({ editDisplay: true })
    }

    closeEdit() {
        //this.props.updateProfileData(this.state.newlinkedAccounts);
        this.setState({ editDisplay: false });
    }

    savelinkedAccounts() {
        const data1 = Object.assign({}, this.state.newlinkedAccounts);
        let data = { linkedAccounts: data1 }
        this.props.saveProfileData(data);
        this.closeEdit();
    }

    handleChange(event) {
        //event.persist()
        //console.log(event);
        const data = Object.assign({}, this.state.newlinkedAccounts)
        data[event.target.name] = event.target.value
        this.setState({
            newlinkedAccounts: data
        })
    }

    goToLinkedin() {
        return window.location.href = this.state.newlinkedAccounts.linkedIn;
    }

    goToGithub() {
        return window.location.href = this.state.newlinkedAccounts.github;
    }

    renderDisplay() {
        return (
            <div className="ui wide column">
                <Button type='button' color='linkedin' onClick={this.goToLinkedin}>
                    <Icon name='linkedin' /> LinkedIn
                </Button>
                <Button type='button' color='black' onClick={this.goToGithub}>
                    <Icon name='github' /> GitHub
                </Button>
                <Button color='black' floated='right'
                    onClick={this.handleClickEdit}>
                    Edit
                </Button>
            </div>
        )
    }

    renderEdit() {
        return (
            <div className='ui sixteen wide column'>
                <ChildSingleInput
                    inputType="text"
                    label="LinkedIn"
                    name="linkedIn"
                    value={this.state.newlinkedAccounts.linkedIn}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter your LinkedIn Url"
                    errorMessage="Please enter a valid LinkedIn Url"
                />
                <ChildSingleInput
                    inputType="text"
                    label="GitHub"
                    name="github"
                    value={this.state.newlinkedAccounts.github}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter your GitHub Url"
                    errorMessage="Please enter a valid GitHub Url"
                />

                <button type="button" className="ui teal button" onClick={this.savelinkedAccounts}>Save</button>
                <button type="button" className="ui button" onClick={this.closeEdit}>Cancel</button>
            </div>
        )
    }

    render() {
        return (
            this.state.editDisplay ? this.renderEdit() : this.renderDisplay()
        )

    }

}