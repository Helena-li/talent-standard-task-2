import React from 'react'
import { SingleInput } from '../Form/SingleInput.jsx';
import { Button, Table, Grid, Icon, Dropdown, Form } from 'semantic-ui-react';
import { TableRowFix3 } from '../Form/TableBody.jsx';
import DatePicker from 'react-datepicker';
import moment from 'moment';

const visaOptions = [
    {
        text: "Citizen",
        value: "Citizen"
    },
    {
        text: "Permanent resident",
        value: "Permanent resident"
    },
    {
        text: "Work visa",
        value: "Work visa"
    },
    {
        text: "Student visa",
        value: "Student visa"
    }
]
export default class VisaStatus extends React.Component {
    constructor(props) {
        super(props)
        //console.log(this.props)
        const getVisaStatus = this.props.visaStatus ? this.props.visaStatus : "";
        const getVisaExpiryDate = this.props.visaExpiryDate ? moment(this.props.visaExpiryDate) : moment();
        this.state = {
            visaStatus: getVisaStatus,
            visaExpiryDate: getVisaExpiryDate
        }
        this.handleDropdownChange = this.handleDropdownChange.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.saveVisa = this.saveVisa.bind(this);
    }

    componentWillReceiveProps(props) {
        //console.log(props)
        this.setState({
            visaStatus: props.visaStatus,
            visaExpiryDate: moment(props.visaExpiryDate)
        });
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        this.setState({
            visaStatus: e.value,
            visaExpiryDate: (e.value == "Citizen" || e.value == "Permanent resident") ? moment() : this.state.visaExpiryDate
        }, this.saveVisa)
    }

    handleChange(event) {
        //event.persist()
        //console.log(event)
        // const data = event.target.value
        this.setState({
            visaExpiryDate: event
        })
    }

    saveVisa() {
        //console.log(this.props.componentId)
        //console.log(this.state.newAddress)
        let data = {
            visaStatus: this.state.visaStatus,
            visaExpiryDate: this.state.visaExpiryDate
        }
        this.props.saveProfileData(data)
    }

    render() {
        //const { nationality } = this.state;
        return (
            <Grid.Column>
                <Grid columns={3}>
                    <Grid.Row>
                        <Grid.Column>
                            <Form.Field>
                                <label>Visa type</label>
                                <Dropdown placeholder='Select your visa status'
                                    selection
                                    options={visaOptions}
                                    value={this.state.visaStatus}
                                    onChange={this.handleDropdownChange}
                                />
                            </Form.Field>
                        </Grid.Column>
                        {(this.state.visaStatus == "Work visa" || this.state.visaStatus == "Student visa") && (
                            <Form.Group>
                                <Form.Field>
                                    <label>Visa expiry date</label>
                                    <DatePicker
                                        selected={this.state.visaExpiryDate}
                                        onChange={this.handleChange}
                                        name='start'
                                    />
                                </Form.Field>
                                <Form.Button type="button" floated="right" color="black" onClick={this.saveVisa} content="Save" />
                            </Form.Group>
                        )}

                    </Grid.Row>
                </Grid>
            </Grid.Column>
        )
    }
}