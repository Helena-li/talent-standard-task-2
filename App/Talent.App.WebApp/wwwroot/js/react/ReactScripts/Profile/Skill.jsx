/* Skill section */
import React from 'react';
import Cookies from 'js-cookie';
import { Button, Table, Grid, Icon, Dropdown } from 'semantic-ui-react';
import { TableRowFix3 } from '../Form/TableBody.jsx';

const skillLevelOptions = [
    {
        text: "Beginner",
        value: "Beginner"
    },
    {
        text: "Intermediate",
        value: "Intermediate"
    },
    {
        text: "Expert",
        value: "Expert"
    }
]
export default class Skill extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props);
        const skillData = props.skillData ?
            Object.assign([], props.skillData)
            : []
        this.state = {
            addNew: false,
            skills: skillData,
            newSkill: {
                level: '',
                name: ''
            },
        }
        this.loadSkill = this.loadSkill.bind(this)
        this.addSkill = this.addSkill.bind(this)
        this.changeSkillName = this.changeSkillName.bind(this)
        this.handleDropdownChange = this.handleDropdownChange.bind(this)
        this.saveAddNew = this.saveAddNew.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.UpdateSkill = this.UpdateSkill.bind(this)
        this.deleteSkill = this.deleteSkill.bind(this)
        this.renderCell = this.renderCell.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        this.setState({
            skills: nextProps.skillData,
        });
    }

    loadSkill() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getSkill',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ skills: res.data })
                this.props.updataState({ skills: res.data })
            }.bind(this)
        })
    }

    //add skill
    addSkill() {
        this.setState({ addNew: true });
    }

    changeSkillName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newSkill)
        data[event.target.name] = event.target.value
        this.setState({
            newSkill: data
        })
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.newSkill)
        data[e.className] = e.value
        this.setState({
            newSkill: data
        })
    }

    saveAddNew() {
        //console.log(this.state);
        const { newSkill } = this.state;
        if (newSkill.name.length == 0 || newSkill.level.length == 0) {
            TalentUtil.notification.show("Add item should not be null", "error", null, null)
        }
        else {
            var cookies = Cookies.get('talentAuthToken');
            $.ajax({
                url: 'http://localhost:60290/profile/profile/addSkill',
                headers: {
                    'Authorization': 'Bearer ' + cookies,
                    'Content-Type': 'application/json'
                },
                type: "POST",
                data: JSON.stringify(newSkill),
                success: function (res) {
                    //console.log(res)
                    if (res.success == true) {
                        TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                        this.loadSkill();
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
    UpdateSkill(data, index) {
        //console.log(this.state);
        const skills = this.state;
        var result = Object.assign({}, skills[index], data);
        //debugger;
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateSkill',
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
                    this.loadSkill();
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

    deleteSkill(id) {
        const data = this.state.skills.find(x => x.id == id);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/deleteSkill',
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
                    this.loadSkill();
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
        const { skills } = this.state;
        //console.log(skills)
        if (skills.length != 0) {
            return this.state.skills.map((x, index) => {
                return (
                    <TableRowFix3 key={x.id}
                        displayRowItem={x}
                        dropdownOptions={skillLevelOptions}
                        updateTableRow={this.UpdateSkill}
                        deleteLanguage={() => this.deleteSkill(x.id)}
                        languagesIndex={index}
                    />
                )
            })

        }
        return null;
    }

    render() {
        const newSkill = this.state;
        return (
            <Grid.Column>
                {this.state.addNew && (
                    <Grid columns={3} divided>
                        <Grid.Row>
                            <Grid.Column>
                                <input onChange={this.changeSkillName}
                                    placeholder='Add skill'
                                    name='name'
                                    value={newSkill.name}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <Dropdown placeholder='Skill Level'
                                    selection
                                    options={skillLevelOptions}
                                    value={newSkill.level}
                                    onChange={this.handleDropdownChange}
                                    className='level'
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
                <Table celled columns={3}>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>Skill</Table.HeaderCell>
                            <Table.HeaderCell>Level</Table.HeaderCell>
                            <Table.HeaderCell>
                                <Button
                                    color='black'
                                    onClick={this.addSkill}
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

