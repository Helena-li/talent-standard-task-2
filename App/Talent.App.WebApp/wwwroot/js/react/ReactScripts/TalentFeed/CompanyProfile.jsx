import React from 'react';
import { Loader } from 'semantic-ui-react';
import Cookies from 'js-cookie';
import { Card, Icon, Dropdown, Checkbox, Accordion, Form, Segment } from 'semantic-ui-react';

export default class CompanyProfile extends React.Component {
    constructor(props) {
        super(props);
        console.log(props)
        const companyDetails = {
            name: "",
            location: "",
            email: "",
            phone: ""
        }
        this.state = {
            companyDetails: companyDetails,
        }
    }

    componentDidMount() {
        this.loadEmployerData();
    };

    loadEmployerData() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getEmployerProfile',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            success: function (res) {
                this.setState({ companyDetails: Object.assign({}, res.employer.companyContact) })
            }.bind(this),
            error: function (res) {
                console.log(res.status)
            }
        })
    }

    render() {
        const { companyDetails } = this.state;
        return (
            <Card>
                <Card.Content>
                    <Card.Header textAlign='center'>
                        <Icon name='picture' circular color='grey' /><br />
                        {companyDetails.name}
                    </Card.Header>
                    <Card.Meta textAlign='center'>
                        <span className='date'>
                            <Icon name='map pin' />
                            {companyDetails.location.city}, {companyDetails.location.country}
                        </span>
                    </Card.Meta>
                    <Card.Description textAlign='center'>We currently do not have specific skills that we desire.</Card.Description>
                </Card.Content>
                <Card.Content extra>
                    <a>
                        <Icon name='phone' />
                        : {companyDetails.phone}
                    </a>
                    <br/>
                    <a>
                        <Icon name='mail' />
                        : {companyDetails.email}
                    </a>
                </Card.Content>
            </Card>)
    }
}