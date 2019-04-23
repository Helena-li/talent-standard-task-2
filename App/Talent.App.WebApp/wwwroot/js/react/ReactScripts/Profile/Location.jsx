import React from 'react'
import Cookies from 'js-cookie'
import { default as Countries } from '../../../../util/jsonFiles/countries.json';
import { ChildSingleInput } from '../Form/SingleInput.jsx';
import { Dropdown, Grid } from 'semantic-ui-react';

const countryOptions = Object.keys(Countries).map(x => ({
    text: x,
    value: x
})
);

export class Address extends React.Component {
    constructor(props) {
        super(props)
        //console.log(props);
        const addressData = props.addressData ?
            Object.assign({}, props.addressData)
            : {
                number: "",
                street: "",
                suburb: "",
                postCode: "",
                city: "",
                country: ""
            }
        this.state = {
            showEditSection: false,
            newAddress: addressData
        }
        this.openEdit = this.openEdit.bind(this)
        this.closeEdit = this.closeEdit.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.saveContact = this.saveContact.bind(this)
        this.renderEdit = this.renderEdit.bind(this)
        this.renderDisplay = this.renderDisplay.bind(this)
        this.handleDropdownChange = this.handleDropdownChange.bind(this)
    }

    componentWillReceiveProps(nextProps) {
        //console.log(nextProps.addressData)
        this.setState({ newAddress: nextProps.addressData });
    }

    openEdit() {
        const addressData = Object.assign({}, this.props.addressData)
        this.setState({
            showEditSection: true,
            newAddress: addressData
        })
    }

    closeEdit() {
        this.setState({
            showEditSection: false
        })
    }

    handleChange(event) {
        //event.persist()
        //console.log(event)
        const data = Object.assign({}, this.state.newAddress)
        data[event.target.name] = event.target.value
        this.setState({
            newAddress: data
        })
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        const data = Object.assign({}, this.state.newAddress)
        data[e.className] = e.value
        this.setState({
            newAddress: data
        })
    }

    saveContact() {
        //console.log(this.props.componentId)
        //console.log(this.state.newAddress)
        const data1 = Object.assign({}, this.state.newAddress)
        let data = { address: data1 }
        this.props.saveProfileData(data)
        this.closeEdit()
    }

    renderEdit() {
        let citiesOptions = [];
        const selectedCountry = this.state.newAddress.country;
        const selectedCity = this.state.newAddress.city;
        if (selectedCountry != "" && selectedCountry != null) {
            citiesOptions = Countries[selectedCountry].map(x => ({ text: x, value: x }))
        }
        return (
            <div className='ui sixteen wide column'>
                <ChildSingleInput
                    inputType="text"
                    label="Number"
                    name="number"
                    value={this.state.newAddress.number}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter your address number"
                    errorMessage="Please enter a valid address number"
                    floated
                />
                <ChildSingleInput
                    inputType="text"
                    label="Street"
                    name="street"
                    value={this.state.newAddress.street}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter your street"
                    errorMessage="Please enter a valid street"
                    floated
                />
                <ChildSingleInput
                    inputType="text"
                    label="Suburb"
                    name="suburb"
                    value={this.state.newAddress.suburb}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter an suburb"
                    errorMessage="Please enter a valid suburb"
                    floated
                />
                <label>Country</label><br />
                <Dropdown placeholder='Country'
                    selection
                    options={countryOptions}
                    value={this.state.newAddress.country}
                    onChange={this.handleDropdownChange}
                    className='country'
                />
                <div>
                    <label>City</label><br />
                    <Dropdown placeholder='City'
                        selection
                        options={citiesOptions}
                        value={this.state.newAddress.city}
                        onChange={this.handleDropdownChange}
                        className='city'
                    />
                </div>
                <ChildSingleInput
                    inputType="text"
                    label="PostCode"
                    name="postCode"
                    value={this.state.newAddress.postCode}
                    controlFunc={this.handleChange}
                    maxLength={80}
                    placeholder="Enter an postCode"
                    errorMessage="Please enter a valid postCode"
                    floated
                />
                <button type="button" className="ui teal button" onClick={this.saveContact}>Save</button>
                <button type="button" className="ui button" onClick={this.closeEdit}>Cancel</button>
            </div>
        )
    }

    renderDisplay() {
        const { number, street, suburb, postCode, city, country } = this.state.newAddress;
        let address = (number && street && suburb && postCode) ? number + ', ' + street + ', ' + suburb + ', ' + postCode : "";

        return (
            <div className='row'>
                <div className="ui sixteen wide column">
                    <React.Fragment>
                        <p>Address: {address}</p>
                        <p>City: {city}</p>
                        <p>Country: {country}</p>
                    </React.Fragment>
                    <button type="button" className="ui right floated teal button" onClick={this.openEdit}>Edit</button>
                </div>
            </div>
        )
    }

    render() {
        return (
            this.state.showEditSection ? this.renderEdit() : this.renderDisplay()
        )
    }

}

export class Nationality extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props);
        const nationalityData = this.props.nationalityData ? this.props.nationalityData : "";
        this.state = {
            nationality: nationalityData
        }
        this.handleDropdownChange = this.handleDropdownChange.bind(this);
        this.saveNationality = this.saveNationality.bind(this);
    }

    componentWillReceiveProps(props) {
        //console.log(props.nationalityData)
        this.setState({ nationality: props.nationalityData });
    }

    handleDropdownChange(event, e) {
        //event.persist()
        //console.log(e)
        //console.log(event)
        this.setState({
            nationality: e.value
        }, this.saveNationality)
    }

    saveNationality() {
        //console.log(this.props.componentId)
        //console.log(this.state.newAddress)
        let data = { nationality: this.state.nationality }
        this.props.saveProfileData(data)
    }

    render() {
        //const { nationality } = this.state;
        return (
            <Grid.Column>
                <Dropdown placeholder='Select your nationality'
                    selection
                    options={countryOptions}
                    value={this.state.nationality}
                    onChange={this.handleDropdownChange}
                />
            </Grid.Column>
        )
    }
}