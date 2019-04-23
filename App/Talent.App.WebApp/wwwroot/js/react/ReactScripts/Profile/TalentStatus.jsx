import React from 'react'
import { Form, Radio, Grid } from 'semantic-ui-react';
import { SingleInput } from '../Form/SingleInput.jsx';

export default class TalentStatus extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props)
        const statusProps = this.props.status ? this.props.status
            : {
                status: "",
                availableDate: null
            };
        this.state = {
            statusProps: statusProps,
            talentstatus: statusProps.status
        }
        this.handleChange = this.handleChange.bind(this);
        this.saveStatus = this.saveStatus.bind(this);
    }

    componentWillReceiveProps(props) {
        //console.log(props.nationalityData)
        this.setState({
            statusProps: props.status ? this.props.status
                : {
                    status: "",
                    availableDate: null
                },
            talentstatus: props.status.status
        });
    }

    handleChange(e, { value }) {
        let data = this.state.talentstatus;
        data = value;
        let propStatus = Object.assign({}, this.state.statusProps, { status: data })
        //console.log(value)
        this.setState({
            talentstatus: value,
            statusProps: propStatus
        }, this.saveStatus)
    }

    saveStatus() {
        let data = { jobSeekingStatus: this.state.statusProps }
        this.props.saveProfileData(data)
    }

    render() {
        return (
            <Grid.Column>
                <Form.Field>
                    Current Status
                </Form.Field>
                <Form.Field>
                    <Radio
                        label='Actively looking for a job'
                        name='radioGroup'
                        value='Actively looking for a job'
                        checked={this.state.talentstatus === 'Actively looking for a job'}
                        onChange={this.handleChange}
                    />
                </Form.Field>
                <Form.Field>
                    <Radio
                        label='Not looking for a job at the moment'
                        name='radioGroup'
                        value='Not looking for a job at the moment'
                        checked={this.state.talentstatus === 'Not looking for a job at the moment'}
                        onChange={this.handleChange}
                    />
                </Form.Field>
                <Form.Field>
                    <Radio
                        label='Currently employed but open to offers'
                        name='radioGroup'
                        value='Currently employed but open to offers'
                        checked={this.state.talentstatus === 'Currently employed but open to offers'}
                        onChange={this.handleChange}
                    />
                </Form.Field>
                <Form.Field>
                    <Radio
                        label='Will be available on later date'
                        name='radioGroup'
                        value='Will be available on later date'
                        checked={this.state.talentstatus === 'Will be available on later date'}
                        onChange={this.handleChange}
                    />
                </Form.Field>
            </Grid.Column>
        )
    }
}