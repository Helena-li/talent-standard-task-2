/* Experience section */
import React from 'react';
import Cookies from 'js-cookie';
import { Button, Table, Grid, Icon, Dropdown, Form } from 'semantic-ui-react';
import { TableRowExperience } from '../Form/TableBody.jsx';
import DatePicker from 'react-datepicker';
import moment from 'moment';

export default class Experience extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props);
        const experienceData = props.experienceData ?
            Object.assign([], props.experienceData)
            : []
        this.state = {
            addNew: false,
            experiences: experienceData,
            newExperience: {
                company: '',
                position: '',
                responsibilities: '',
                start: moment(),
                end: moment()
            },
        }
        this.loadExperience = this.loadExperience.bind(this)
        this.addExperience = this.addExperience.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.saveAddNew = this.saveAddNew.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.UpdateExperience = this.UpdateExperience.bind(this)
        this.deleteExperience = this.deleteExperience.bind(this)
        this.renderCell = this.renderCell.bind(this)
        this.handleEndDateChange = this.handleEndDateChange.bind(this)
        this.handleStartDateChange = this.handleStartDateChange.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        this.setState({
            experiences: nextProps.experienceData,
        });
    }

    loadExperience() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ experiences: res.data })
                this.props.updataState({ experience: res.data })
            }.bind(this)
        })
    }

    //add Experience
    addExperience() {
        this.setState({ addNew: true });
    }

    handleChange(event) {
        event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newExperience)
        data[event.target.name] = event.target.value
        this.setState({
            newExperience: data
        })
    }

    handleStartDateChange(event) {
        const data = Object.assign({}, this.state.newExperience)
        data.start = event
        this.setState({ newExperience: data})
    }

    handleEndDateChange(event) {
        const data = Object.assign({}, this.state.newExperience)
        data.end = event
        this.setState({ newExperience: data })
    }
   
    saveAddNew() {
        //console.log(this.state);
        const addNewExperience = this.state.newExperience;
        //let newExperience = { experience: addNewExperience };
        let isNull = addNewExperience.company.length == 0 || addNewExperience.position.length == 0
            || addNewExperience.responsibilities == 0 || addNewExperience.start.length == 0 || addNewExperience.end.length == 0;
        if (isNull) {
            TalentUtil.notification.show("Add item should not be null", "error", null, null)
        }
        else {
            var cookies = Cookies.get('talentAuthToken');
            $.ajax({
                url: 'http://localhost:60290/profile/profile/addExperience',
                headers: {
                    'Authorization': 'Bearer ' + cookies,
                    'Content-Type': 'application/json'
                },
                type: "POST",
                data: JSON.stringify(addNewExperience),
                success: function (res) {
                    //console.log(res)
                    if (res.success == true) {
                        TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                        this.loadExperience();
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
        this.closeAdd();
    }

    closeAdd() {
        this.setState({ addNew: false });
    }

    UpdateExperience(data, index) {
        //console.log(this.state);
        const { experiences } = this.state;
        var result = Object.assign({}, experiences[index], data);
        //debugger;
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            data: JSON.stringify(result),
            success: function (res) {
                console.log(res)
                if (res.success == true) {
                    TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                    this.loadExperience();
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

    deleteExperience(id) {
        const data = this.state.experiences.find(x => x.id == id);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/deleteExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            data: JSON.stringify(data),
            success: function (res) {
                console.log(res)
                if (res.success == true) {
                    TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                    this.loadExperience();
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

    renderCell() {
        const { experiences } = this.state;
        //console.log(experiences)
        if (experiences.length != 0) {
            return this.state.experiences.map((x, index) => {
                return (
                    <TableRowExperience key={x.id}
                        displayRowItem={x}
                        updateTableRow={this.UpdateExperience}
                        deleteExperience={() => this.deleteExperience(x.id)}
                        experienceIndex={index}
                    />
                )
            })
        }
        return null;
    }

    render() {
        const { newExperience } = this.state;
        return (
            <Grid.Column>
                {this.state.addNew && (
                    <Grid divided>
                        <Grid.Row columns={2}>
                            <Grid.Column>
                                <label>Company:</label>
                                <input type='text'
                                    onChange={this.handleChange}
                                    name='company'
                                    value={newExperience.company} />
                            </Grid.Column>
                            <Grid.Column>
                                <label>Position:</label>
                                <input type='text'
                                    onChange={this.handleChange}
                                    name='position'
                                    value={newExperience.position} />
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={2}>
                            <Grid.Column>
                                <Form.Field>
                                    <label>Start Date:</label>
                                    <DatePicker
                                        selected={newExperience.start}
                                        onChange={this.handleStartDateChange}
                                        name='start'
                                    />
                                </Form.Field>
                                
                            </Grid.Column>
                            <Grid.Column>
                                <Form.Field>
                                <label>End Date:</label>
                                <DatePicker
                                    selected={newExperience.end}
                                        onChange={this.handleEndDateChange}
                                    name='end'
                                    />
                                </Form.Field>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column>
                                <label>Responsibilities:</label>
                                <input type='text'
                                    onChange={this.handleChange}
                                    name='responsibilities'
                                    value={newExperience.responsibilities} />
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column>
                                <button type="button" className="ui teal button" onClick={this.saveAddNew}>Add</button>
                                <button type="button" className="ui button" onClick={this.closeAdd}>Cancel</button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                )
                }
                <Table celled columns={6} fixed>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>Company</Table.HeaderCell>
                            <Table.HeaderCell>Position</Table.HeaderCell>
                            <Table.HeaderCell>Responsibilities</Table.HeaderCell>
                            <Table.HeaderCell>Start</Table.HeaderCell>
                            <Table.HeaderCell>End</Table.HeaderCell>
                            <Table.HeaderCell>
                                <Button
                                    color='black'
                                    onClick={this.addExperience}
                                    floated='right'
                                    type='button'
                                >
                                    <Icon name='add' />
                                    Add New
                                </Button>
                            </Table.HeaderCell>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {this.renderCell()}
                    </Table.Body>
                </Table>
            </Grid.Column>
        )
    }
}
