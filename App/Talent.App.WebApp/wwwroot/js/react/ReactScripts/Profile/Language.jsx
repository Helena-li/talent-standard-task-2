/* Language section */
import React from 'react';
import Cookies from 'js-cookie';
import { Button, Table,Grid, Icon, Dropdown } from 'semantic-ui-react'; 
import { TableRowFix3 } from '../Form/TableBody.jsx';

const languageOptions = [
    {
        text: "Basic",
        value: "Basic"
    },
    {
        text: "Conversational",
        value: "Conversational"
    },
    {
        text: "Fluent",
        value: "Fluent"
    },
    {
        text: "Native/Bilingual",
        value: "Native/Bilingual"
    }
]
export default class Language extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props);
        const languageData = props.languageData ?
            Object.assign([], props.languageData)
            : []
        this.state = {
            addNew: false,
            languages: languageData,
            newLanguage: {
                level: '',
                name: ''
            },
        }
       
        this.loadLanguage = this.loadLanguage.bind(this)
        this.addLanguage = this.addLanguage.bind(this)
        this.changeLanguageName = this.changeLanguageName.bind(this)
        this.handleDropdownChange = this.handleDropdownChange.bind(this)
        this.saveAddNew = this.saveAddNew.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.UpdateLanguage = this.UpdateLanguage.bind(this)
        this.deleteLanguage = this.deleteLanguage.bind(this)
        this.renderCell = this.renderCell.bind(this)
    }

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        //let index = nextProps.languageData.map(x => x.id)
        //console.log(index)
        this.setState({
            languages: nextProps.languageData,
        });
    }

    loadLanguage() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getLanguage',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ languages: res.data })
                this.props.updataState({languages: res.data})
            }.bind(this)
        })
    }

    addLanguage() {
        this.setState({ addNew: true });
    }

    changeLanguageName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newLanguage)
        data[event.target.name] = event.target.value
        this.setState({
            newLanguage: data
        })
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.newLanguage)
        data[e.className] = e.value
        this.setState({
            newLanguage: data
        })
    }

    saveAddNew() {
        //console.log(this.state);
        const { newLanguage } = this.state;
        debugger;
        if (newLanguage.name.length==0||newLanguage.level.length==0) {
            TalentUtil.notification.show("Add item should not be null", "error", null, null)
        }
        else {
            var cookies = Cookies.get('talentAuthToken');
            $.ajax({
                url: 'http://localhost:60290/profile/profile/addLanguage',
                headers: {
                    'Authorization': 'Bearer ' + cookies,
                    'Content-Type': 'application/json'
                },
                type: "POST",
                data: JSON.stringify(newLanguage),
                success: function (res) {
                    //console.log(res)
                    if (res.success == true) {
                        TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                        this.loadLanguage();
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
        this.setState({
            addNew: false,
            newLanguage: {
                level: '',
                name: ''
            },
        });
    }

    UpdateLanguage(data, index) {
        //console.log(this.state);
        const languages = this.state;
        var result = Object.assign({}, languages[index], data);
        //debugger;
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateLanguage',
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
                    this.loadLanguage();
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

    deleteLanguage(id) {
        const data = this.state.languages.find(x=>x.id==id);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/deleteLanguage',
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
                    this.loadLanguage();
                } else {
                    TalentUtil.notification.show("Profile did not update successfully", "error", null, null)
                }

            }.bind(this),
            error: function (res, a, b) {
                console.log(res)
                console.log(a)
                console.log(b)
            }
        })
        
    }

    renderCell() {
        const { languages } = this.state;
        //console.log(skills)
        if (languages.length != 0) {
            return this.state.languages.map((x, index) => {
                return (
                    <TableRowFix3 key={x.id}
                        displayRowItem={x}
                        dropdownOptions={languageOptions}
                        updateTableRow={this.UpdateLanguage}
                        deleteLanguage={() => this.deleteLanguage(x.id)}
                        languagesIndex={index}
                    />
                )
            })

        }
        return null;
    }
    
    render() {
        const newLanguage = this.state;
        return (
            <Grid.Column>
                {this.state.addNew && (
                    <Grid columns={3} divided>
                        <Grid.Row>
                            <Grid.Column>
                                <input onChange={this.changeLanguageName}
                                    placeholder='Add Language'
                                    name='name'
                                    value={newLanguage.name}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <Dropdown placeholder='Language Level'
                                    selection
                                    options={languageOptions}
                                    value={newLanguage.level}
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
                            <Table.HeaderCell>Language</Table.HeaderCell>
                            <Table.HeaderCell>Level</Table.HeaderCell>
                            <Table.HeaderCell>
                                <Button
                                    color='black'
                                    onClick={this.addLanguage}
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