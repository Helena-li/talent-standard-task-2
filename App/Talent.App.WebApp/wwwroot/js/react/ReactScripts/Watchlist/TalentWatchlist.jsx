import React from 'react'
import Cookies from 'js-cookie'
import { BodyWrapper, loaderData } from '../Layout/BodyWrapper.jsx'
import TalentCard from '../TalentFeed/TalentCard.jsx'

export default class TalentWatchlist extends React.Component {
    constructor(props) {
        super(props)

        let loader = loaderData
        loader.allowedUsers.push("Employer")
        loader.allowedUsers.push("Recruiter")

        this.state = {
            loaderData: loader
        }
        this.init = this.init.bind(this);
    }   

    init() {
        let loaderData = TalentUtil.deepCopy(this.state.loaderData)
        loaderData.isLoading = false;
        this.setState({ loaderData });//comment this
    }

    componentDidMount() {
        this.init()
    };

    //loadWatchlist()   url: 'http://localhost:51689/listing/listing/getWatchlist',
 
    render() {
       
      
        return (
            <BodyWrapper reload={this.init} loaderData={this.state.loaderData} >
                <div className="ui container">Your code goes here</div>
            </BodyWrapper>
        )
    }
}