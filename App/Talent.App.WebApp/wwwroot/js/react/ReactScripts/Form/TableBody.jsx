import React from 'react';
import PropTypes from 'prop-types';
import { Button, Table, Icon, Dropdown, Form, Grid } from 'semantic-ui-react';
import DatePicker from 'react-datepicker';
import moment from 'moment';

export class TableRow extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            rowEdit: false,
            displayRowItem: this.props.displayRowItem
        }
    };

    openEdit() {
        this.setstate({ rowEdit: true })
    }

    closeEdit() {
        this.setstate({ rowEdit: false })
    }

    handleChange(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[event.target.name] = event.target.value
        this.setState({
            displayRowItem: data
        })
    }

    renderDisplay() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                {Object.keys(item).map(x => {
                    if (x != "id" && item[x] != null) {
                        return <Table.Cell>{item[x]}</Table.Cell>
                    }
                })
                }

                <Table.Cell textAlign='right'>
                    <Icon name='pencil' onClick={this.openEdit} />
                    <Icon name='delete' onClick={this.deleteLanguage} />
                </Table.Cell>
            </Table.Row >
        )
    }

    renderEdit() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                {Object.keys(item).map(x => {
                    if (x != "id" && item[x] != null) {
                        return
                        <Table.Cell>
                            <input type='text'
                                onChange={this.handleChange}
                                name={x}
                                value={item[x]} />
                        </Table.Cell>
                    }
                })
                }

                <Table.Cell textAlign='right'>
                    <Button basic color='blue' content='Update' onClick={this.props.updateTableRow} />
                    <Button basic color='red' content='Cancle' onClick={this.closeEdit} />
                </Table.Cell>
            </Table.Row >
        )
    }

    render() {

        return (
            this.state.rowEdit ? this.renderEdit() : this.renderDisplay()
        )

    }
}

export class TableRowExperience extends React.Component {

    constructor(props) {
        super(props);
        const experience = this.props.displayRowItem;
        experience.start = moment(experience.start)
        experience.end = moment(experience.end)
        this.state = {
            rowEdit: false,
            experienceIndex: this.props.experienceIndex,
            displayRowItem: experience
        }
        this.openEdit = this.openEdit.bind(this);
        this.closeEdit = this.closeEdit.bind(this);
        this.renderDisplay = this.renderDisplay.bind(this);
        this.renderEdit = this.renderEdit.bind(this);
        this.handleChange = this.handleChange.bind(this)
        this.updateExperience = this.updateExperience.bind(this)
        this.handleStartDateChange = this.handleStartDateChange.bind(this)
        this.handleEndDateChange = this.handleEndDateChange.bind(this)
    };

    openEdit() {
        this.setState({ rowEdit: true })
    }

    closeEdit() {
        this.setState({ rowEdit: false })
    }

    updateExperience() {
        this.props.updateTableRow(this.state.displayRowItem, this.state.experienceIndex);
        this.closeEdit();
    }

    handleChange(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[event.target.name] = event.target.value
        this.setState({
            displayRowItem: data
        })
    }

    handleStartDateChange(event) {
        const data = Object.assign({}, this.state.displayRowItem)
        data.start = event
        this.setState({ displayRowItem: data })
    }

    handleEndDateChange(event) {
        const data = Object.assign({}, this.state.displayRowItem)
        data.end = event
        this.setState({ displayRowItem: data })
    }

    renderDisplay() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                <Table.Cell>{item.company}</Table.Cell>
                <Table.Cell>{item.position}</Table.Cell>
                <Table.Cell>{item.responsibilities}</Table.Cell>
                <Table.Cell>{item.start.format('DD/MM/YYYY')}</Table.Cell>
                <Table.Cell>{item.end.format('DD/MM/YYYY')}</Table.Cell>
                <Table.Cell textAlign='right'>
                    <Icon name='pencil' onClick={this.openEdit} />
                    <Icon name='delete' onClick={this.props.deleteExperience} />
                </Table.Cell>
            </Table.Row >
        )
    }

    renderEdit() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                <Table.Cell colSpan={6}>
                    <Grid divided='vertically'>
                        <Grid.Row columns={2}>
                            <Grid.Column>
                                <label>Company:</label>
                                <input type='text'
                                    onChange={this.handleChange}
                                    name='company'
                                    value={item.company} />
                            </Grid.Column>
                            <Grid.Column>
                                <label>Position:</label>
                                <input type='text'
                                    onChange={this.handleChange}
                                    name='position'
                                    value={item.position} />
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={2}>
                            <Grid.Column>
                                <Form.Field>
                                    <label>Start Date:</label>
                                    <DatePicker
                                        selected={item.start}
                                        onChange={this.handleStartDateChange}
                                    />
                                </Form.Field>
                            </Grid.Column>
                            <Grid.Column>
                                <Form.Field>
                                    <label>End Date:</label>
                                    <DatePicker
                                        selected={item.end}
                                        onChange={this.handleEndDateChange}
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
                                    value={item.responsibilities} />
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column>
                                <Button basic color='blue' content='Update' onClick={this.updateExperience} />
                                <Button basic color='red' content='Cancle' onClick={this.closeEdit} />
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                </Table.Cell>
            </Table.Row >
        )
    }

    render() {

        return (
            this.state.rowEdit ? this.renderEdit() : this.renderDisplay()
        )

    }
}

export class TableRowCertification extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            rowEdit: false,
            certificationIndex: this.props.certificationIndex,
            displayRowItem: this.props.displayRowItem
        }
        this.openEdit = this.openEdit.bind(this);
        this.closeEdit = this.closeEdit.bind(this);
        this.renderDisplay = this.renderDisplay.bind(this);
        this.renderEdit = this.renderEdit.bind(this);
        this.handleChange = this.handleChange.bind(this)
        this.updateCertification = this.updateCertification.bind(this)
    };

    openEdit() {
        this.setState({ rowEdit: true })
    }

    closeEdit() {
        this.setState({ rowEdit: false })
    }

    updateCertification() {
        this.props.updateTableRow(this.state.displayRowItem, this.state.certificationIndex);
        this.closeEdit();
    }

    handleChange(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[event.target.name] = event.target.value
        this.setState({
            displayRowItem: data
        })
    }

    renderDisplay() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                <Table.Cell>{item.certificationName}</Table.Cell>
                <Table.Cell>{item.certificationFrom}</Table.Cell>
                <Table.Cell>{item.certificationYear}</Table.Cell>
                <Table.Cell textAlign='right'>
                    <Icon name='pencil' onClick={this.openEdit} />
                    <Icon name='delete' onClick={this.props.deleteCertification} />
                </Table.Cell>
            </Table.Row >
        )
    }

    renderEdit() {
        const item = this.state.displayRowItem
        return (
            < Table.Row >
                <Table.Cell>
                    <input type='text'
                        onChange={this.handleChange}
                        name='certificationName'
                        value={item.certificationName} />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.handleChange}
                        name='certificationFrom'
                        value={item.certificationFrom} />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.handleChange}
                        name='certificationYear'
                        value={item.certificationYear} />
                </Table.Cell>
                <Table.Cell textAlign='right'>
                    <Button basic color='blue' content='Update' onClick={this.updateCertification} />
                    <Button basic color='red' content='Cancle' onClick={this.closeEdit} />
                </Table.Cell>
            </Table.Row >
        )
    }

    render() {

        return (
            this.state.rowEdit ? this.renderEdit() : this.renderDisplay()
        )

    }
}

export class TableRowEducation extends React.Component {

    constructor(props) {
        super(props);
        //console.log(props)
        this.state = {
            rowEdit: false,
            educationIndex: this.props.educationIndex,
            displayRowItem: this.props.displayRowItem,
        }
        this.openRowEdit = this.openRowEdit.bind(this);
        this.closeRowEdit = this.closeRowEdit.bind(this);
        this.renderDisplay = this.renderDisplay.bind(this);
        this.renderEdit = this.renderEdit.bind(this);
        this.changeName = this.changeName.bind(this)
        this.ChangeDropdown = this.ChangeDropdown.bind(this)
        this.updateEducation = this.updateEducation.bind(this)
    };

    componentWillReceiveProps(nextProps) {
        //this.setState({
        //    displayRowItem: this.props.displayRowItem,
        //    languagesIndex: this.props.languagesIndex
        //});
        //console.log(nextProps)
    }

    openRowEdit() {
        this.setState({ rowEdit: true })
    }

    closeRowEdit() {
        this.setState({ rowEdit: false })
    }

    changeName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[event.target.name] = event.target.value
        this.setState({
            displayRowItem: data
        })
    }

    ChangeDropdown(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[e.className] = e.value
        this.setState({
            displayRowItem: data
        })
    }

    updateEducation() {
        this.props.updateTableRow(this.state.displayRowItem, this.state.educationIndex);
        this.closeRowEdit();
    }

    renderDisplay() {
        const item = this.state.displayRowItem;
        return (
            < Table.Row columns={6}>
                <Table.Cell>{item.country}</Table.Cell>
                <Table.Cell>{item.instituteName}</Table.Cell>
                <Table.Cell>{item.title}</Table.Cell>
                <Table.Cell>{item.degree}</Table.Cell>
                <Table.Cell>{item.yearOfGraduation}</Table.Cell>
                <Table.Cell textAlign='right'>
                    <Icon name='pencil' onClick={this.openRowEdit} />
                    <Icon name='delete' onClick={this.props.deleteEducation} />
                </Table.Cell>
            </Table.Row >
        )
    }

    renderEdit() {
        const item = this.state.displayRowItem
        //console.log(item)
        return (
            < Table.Row columns={6}>
                <Table.Cell>
                    <Dropdown
                        selection
                        options={this.props.dropdownOptions}
                        value={item.country}
                        onChange={this.ChangeDropdown}
                        className='country'
                    />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.changeName}
                        name='instituteName'
                        value={item.instituteName} />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.changeName}
                        name='title'
                        value={item.title} />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.changeName}
                        name='degree'
                        value={item.degree} />
                </Table.Cell>
                <Table.Cell>
                    <input type='text'
                        onChange={this.changeName}
                        name='yearOfGraduation'
                        value={item.yearOfGraduation} />
                </Table.Cell>
                <Table.Cell textAlign='right'>
                    <Button basic color='blue' content='Update' onClick={this.updateEducation} />
                    <Button basic color='red' content='Cancle' onClick={this.closeRowEdit} />
                </Table.Cell>
            </Table.Row >
        )
    }

    render() {

        return (
            this.state.rowEdit ? this.renderEdit() : this.renderDisplay()
        )

    }
}

//for skill and language
//props:displayRowItem,deleteLanguage,dropdownOptions,updateTableRow
export class TableRowFix3 extends React.Component {

    constructor(props) {
        super(props);
        //console.log(props)
        this.state = {
            rowEdit: false,
            languagesIndex: this.props.languagesIndex,
            displayRowItem: this.props.displayRowItem ? this.props.displayRowItem :
                {
                    level: '',
                    name: ''
                }
        }
        this.openRowEdit = this.openRowEdit.bind(this);
        this.closeRowEdit = this.closeRowEdit.bind(this);
        this.renderDisplay = this.renderDisplay.bind(this);
        this.renderEdit = this.renderEdit.bind(this);
        this.changeName = this.changeName.bind(this)
        this.ChangeDropdown = this.ChangeDropdown.bind(this)
        this.updateLanguage = this.updateLanguage.bind(this)
    };

    //componentWillReceiveProps(nextProps) {
    //    this.setState({
    //        displayRowItem: this.props.displayRowItem,
    //        languagesIndex: this.props.languagesIndex
    //    });
    //    console.log(nextProps)
    //}

    openRowEdit() {
        this.setState({ rowEdit: true })
    }

    closeRowEdit() {
        this.setState({ rowEdit: false })
    }

    changeName(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[event.target.name] = event.target.value
        this.setState({
            displayRowItem: data
        })
    }

    ChangeDropdown(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.displayRowItem)
        data[e.className] = e.value
        this.setState({
            displayRowItem: data
        })
    }

    updateLanguage() {
        this.props.updateTableRow(this.state.displayRowItem, this.state.languagesIndex);
        this.closeRowEdit();
    }

    renderDisplay() {
        const item = this.state.displayRowItem;
        return (
            < Table.Row >
                <Table.Cell>{item.name}</Table.Cell>
                <Table.Cell>{item.level}</Table.Cell>
                <Table.Cell textAlign='right'>
                    <Icon name='pencil' onClick={this.openRowEdit} />
                    <Icon name='delete' onClick={this.props.deleteLanguage} />
                </Table.Cell>
            </Table.Row >
        )
    }

    renderEdit() {
        const item = this.state.displayRowItem
        //console.log(item)
        return (
            < Table.Row >
                <Table.Cell>
                    <input type='text'
                        onChange={this.changeName}
                        name='name'
                        value={item.name} />
                </Table.Cell>
                <Table.Cell>
                    <Dropdown
                        selection
                        options={this.props.dropdownOptions}
                        value={item.level}
                        onChange={this.ChangeDropdown}
                        className='level'
                    />
                </Table.Cell>
                <Table.Cell textAlign='right'>
                    <Button basic color='blue' content='Update' onClick={this.updateLanguage} />
                    <Button basic color='red' content='Cancle' onClick={this.closeRowEdit} />
                </Table.Cell>
            </Table.Row >
        )
    }

    render() {

        return (
            this.state.rowEdit ? this.renderEdit() : this.renderDisplay()
        )

    }
}