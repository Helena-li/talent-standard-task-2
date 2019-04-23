/* Self introduction section */
import React, { Component } from 'react';
import Cookies from 'js-cookie'
import { Button, Table, Grid, Icon, Dropdown, Form } from 'semantic-ui-react';

export default class SelfIntroduction extends React.Component {
    constructor(props) {
        super(props);
        const summary = props.summary ? props.summary : '';
        const description = props.description ? props.description : '';
        this.state = {
            summary: summary,
            description: description,
        }
        this.handleSumChange = this.handleSumChange.bind(this)
        this.handleDesChange = this.handleDesChange.bind(this)
        this.saveDescription = this.saveDescription.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        this.setState({
            summary: nextProps.summary,
            description: nextProps.description,
        });
    }

    handleSumChange(event) {
        const data = event.target.value
        this.setState({
            summary: data
        })
    }

    handleDesChange(event) {
        const data = event.target.value
        this.setState({
            description: data
        })
    }

    saveDescription() {
        const { summary, description } = this.state
        if ((!summary) || (!description)) {
            TalentUtil.notification.show("description or summary should not be null", "error", null, null)
        }
        else if (summary.length == 0 || description.length == 0) {
            var a = summary.length
            var b = description.length
            TalentUtil.notification.show("description or summary should not be null", "error", null, null)
        }
        else if (summary.length > 150) {
            TalentUtil.notification.show("the number of the characters in summary should < 150", "error", null, null)
        }
        else if (description.length < 150 || description.length > 600) {
            TalentUtil.notification.show("the number of the characters in description should between 150 and 600", "error", null, null)
        }
        else {
            let data = { summary: summary, description: description }
            this.props.updateProfileData(data);
        }

    }

    render() {
        return (
            <Grid.Column >
                <div className="field" >
                    <input type='text'
                        name='summary'
                        value={this.state.summary}
                        onChange={this.handleSumChange}
                        maxLength={150}
                        placeholder="Please provide a short summary about yourself"
                    />
                </div>
                <p>Summary must be no more than 150 characters</p>
                <div className="field" >
                    <textarea
                        name='description'
                        value={this.state.description}
                        onChange={this.handleDesChange}
                        maxLength={600}
                        placeholder="Please tell us about any hobbies, additional experitse or anything you'd like to add."
                    />
                </div>
                <p>Description must be between 150-600 characters</p>
                <button type="button" className="ui teal button" onClick={this.saveDescription}>Save</button>
            </Grid.Column>

        )
    }
}



