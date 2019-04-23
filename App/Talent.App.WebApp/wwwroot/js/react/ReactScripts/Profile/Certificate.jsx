/* Certificate section */
import React from 'react';
import Cookies from 'js-cookie';
import { Button, Table, Grid, Icon, Dropdown } from 'semantic-ui-react';
import { TableRowCertification } from '../Form/TableBody.jsx';

export default class Certificate extends React.Component {

    constructor(props) {
        super(props)
        const certificateData = props.certificateData ?
            Object.assign([], props.certificateData)
            : []
        this.state = {
            addNew: false,
            certifications: certificateData,
            newCertification: {
                certificationName: '',
                certificationFrom: '',
                certificationYear: ''
            },
        }
        this.loadCertification = this.loadCertification.bind(this)
        this.addCertification = this.addCertification.bind(this)
        this.changeCertificationName = this.changeCertificationName.bind(this)
        //this.handleDropdownChange = this.handleDropdownChange.bind(this)
        this.saveAddNew = this.saveAddNew.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.UpdateCertification = this.UpdateCertification.bind(this)
        this.deleteCertification = this.deleteCertification.bind(this)
        this.renderCell = this.renderCell.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.languageData)
        this.setState({
            certifications: nextProps.certificateData,
        });
    }

    loadCertification() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getCertification',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            success: function (res) {
                this.setState({ certifications: res.data })
                this.props.updataState({ certifications: res.data })
            }.bind(this)
        })
    }

    //add certifications
    addCertification() {
        this.setState({ addNew: true });
    }

    changeCertificationName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newCertification)
        data[event.target.name] = event.target.value
        this.setState({
            newCertification: data
        })
    }

    //handleDropdownChange(event, e) {
    //    //event.persist()
    //    //console.log(e)
    //    //console.log(event)
    //    const data = Object.assign({}, this.state.newSkill)
    //    data[e.className] = e.value
    //    this.setState({
    //        newSkill: data
    //    })
    //}

    saveAddNew() {
        //console.log(this.state);
        const { newCertification } = this.state;
        if (newCertification.certificationFrom.length == 0 || newCertification.certificationName.length == 0||newCertification.certificationYear.length==0) {
            TalentUtil.notification.show("Add item should not be null", "error", null, null)
        }
        else {
            var cookies = Cookies.get('talentAuthToken');
            $.ajax({
                url: 'http://localhost:60290/profile/profile/addCertification',
                headers: {
                    'Authorization': 'Bearer ' + cookies,
                    'Content-Type': 'application/json'
                },
                type: "POST",
                data: JSON.stringify(newCertification),
                success: function (res) {
                    //console.log(res)
                    if (res.success == true) {
                        TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                        this.loadCertification();
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
    UpdateCertification(data, index) {
        //console.log(this.state);
        const certifications = this.state;
        var result = Object.assign({}, certifications[index], data);
        //debugger;
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/updateCertification',
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
                    this.loadCertification();
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

    deleteCertification(id) {
        const data = this.state.certifications.find(x => x.id == id);
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/deleteCertification',
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
                    this.loadCertification();
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
        const { certifications } = this.state;
        //console.log(skills)
        if (certifications.length != 0) {
            return this.state.certifications.map((x, index) => {
                return (
                    <TableRowCertification key={x.id}
                        displayRowItem={x}
                        updateTableRow={this.UpdateCertification}
                        deleteCertification={() => this.deleteCertification(x.id)}
                        certificationIndex={index}
                    />
                )
            })

        }
        return null;
    }

    render() {
        const newCertification = this.state;
        return (
            <Grid.Column>
                {this.state.addNew && (
                    <Grid columns={4} divided>
                        <Grid.Row>
                            <Grid.Column>
                                <input onChange={this.changeCertificationName}
                                    placeholder='Add CertificationName'
                                    name='certificationName'
                                    value={newCertification.certificationName}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <input onChange={this.changeCertificationName}
                                    placeholder='Certification From'
                                    name='certificationFrom'
                                    value={newCertification.certificationFrom}
                                />
                            </Grid.Column>
                            <Grid.Column>
                                <input onChange={this.changeCertificationName}
                                    placeholder='Certification Year'
                                    name='certificationYear'
                                    value={newCertification.certificationYear}
                                />
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
                <Table celled columns={4}>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>Certification Name</Table.HeaderCell>
                            <Table.HeaderCell>Certification From</Table.HeaderCell>
                            <Table.HeaderCell>Certification Year</Table.HeaderCell>
                            <Table.HeaderCell>
                                <Button
                                    color='black'
                                    onClick={this.addCertification}
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

