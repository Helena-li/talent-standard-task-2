/* Education section */
import React from 'react';
import Cookies from 'js-cookie';
import { default as Countries } from '../../../../../wwwroot/util/jsonFiles/countries.json'
import { Button, Table, Grid, Icon, Dropdown } from 'semantic-ui-react';
import { TableRowEducation } from '../Form/TableBody.jsx';

const countryOptions = Object.keys(Countries).map(x => ({
    text: x,
    value: x
})
);
export default class Education extends React.Component {
    constructor(props) {
        super(props)
        const educationData = props.educationData ?
            Object.assign([], props.educationData)
            : []
        this.state = {
            addNew: false,
            education: educationData,
            newEducation: {
                country: '',
                instituteName: '',
                title: '',
                degree: '',
                yearOfGraduation: ''
            },
        }
        this.loadEducation = this.loadEducation.bind(this)
        this.addEducation = this.addEducation.bind(this)
        this.changeEducationName = this.changeEducationName.bind(this)
        this.handleDropdownChange = this.handleDropdownChange.bind(this)
        this.saveAddNew = this.saveAddNew.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.updateEducation = this.updateEducation.bind(this)
        this.deleteEducation = this.deleteEducation.bind(this)
        this.renderCell = this.renderCell.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        this.setState({
            education: nextProps.educationData,
        });
    }

    loadEducation() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getEducation',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ education: res.data })
                this.props.updataState({ education: res.data })
            }.bind(this)
        })
    }

    //add skill
    addEducation() {
        this.setState({ addNew: true });
    }

    changeEducationName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newEducation)
        data[event.target.name] = event.target.value
        this.setState({
            newEducation: data
        })
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.newEducation)
        data[e.className] = e.value
        this.setState({
            newEducation: data
        })
    }

    saveAddNew() {
        //console.log(this.state);
        const { newEducation } = this.state;
        if (newEducation.country.length == 0 || newEducation.instituteName.length == 0 || newEducation.title.length == 0
            || newEducation.degree.length == 0 || newEducation.yearOfGraduation.length == 0) {
            TalentUtil.notification.show("Add item should not be null", "error", null, null)
        }
        else {
            var cookies = Cookies.get('talentAuthToken');
            $.ajax({
                url: 'http://localhost:60290/profile/profile/addEducation',
                headers: {
                    'Authorization': 'Bearer ' + cookies,
                    'Content-Type': 'application/json'
                },
                type: "POST",
                data: JSON.stringify(newEducation),
                success: function (res) {
                    //console.log(res)
                    if (res.success == true) {
                        TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                        this.loadEducation();
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

    //update skill
    updateEducation(data, index) {
        //console.log(this.state);
        const education = this.state;
        var result = Object.assign({}, education[index], data);
        //debugger;
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateEducation',
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
                    this.loadEducation();
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

    deleteEducation(id) {
        const data = this.state.education.find(x => x.id == id);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/deleteEducation',
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
                    this.loadEducation();
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
        const { education } = this.state;
        //console.log(skills)
        if (education.length != 0) {
            return this.state.education.map((x, index) => {
                return (
                    <TableRowEducation key={x.id}
                        displayRowItem={x}
                        dropdownOptions={countryOptions}
                        updateTableRow={this.updateEducation}
                        deleteEducation={() => this.deleteEducation(x.id)}
                        educationIndex={index}
                    />
                )
            })

        }
        return null;
    }

    render() {
        const newEducation = this.state;
        return (
            <Grid.Column>
                {this.state.addNew && (
                    <Grid divided>
                        <Grid.Row columns={3}>
                            <Grid.Column>
                                <Dropdown placeholder='Country'
                                    selection
                                    options={countryOptions}
                                    value={newEducation.country}
                                    onChange={this.handleDropdownChange}
                                    className='country'
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <input onChange={this.changeEducationName}
                                    placeholder='Add institute name'
                                    name='instituteName'
                                    value={newEducation.instituteName}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <input onChange={this.changeEducationName}
                                    placeholder='Add title'
                                    name='title'
                                    value={newEducation.title}
                                />
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={3}>
                            <Grid.Column>
                                <input onChange={this.changeEducationName}
                                    placeholder='Add degree'
                                    name='degree'
                                    value={newEducation.degree}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <input onChange={this.changeEducationName}
                                    placeholder='Add year of graduation'
                                    name='yearOfGraduation'
                                    value={newEducation.yearOfGraduation}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <button type="button" className="ui teal button" onClick={this.saveAddNew}>Add</button>
                                <button type="button" className="ui button" onClick={this.closeAdd}>Cancel</button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                )
                }
                <Table celled columns={6}>
                    <Table.Header >
                        <Table.Row>
                            <Table.HeaderCell>Country</Table.HeaderCell>
                            <Table.HeaderCell>Institute name</Table.HeaderCell>
                            <Table.HeaderCell>Title</Table.HeaderCell>
                            <Table.HeaderCell>Degree</Table.HeaderCell>
                            <Table.HeaderCell>Year of graduation</Table.HeaderCell>
                            <Table.HeaderCell>
                                <Button
                                    color='black'
                                    onClick={this.addEducation}
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
