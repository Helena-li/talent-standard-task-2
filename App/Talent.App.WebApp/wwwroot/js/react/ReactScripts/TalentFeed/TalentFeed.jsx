import React from 'react';
import ReactDOM from 'react-dom';
import Cookies from 'js-cookie'
import TalentCard from '../TalentFeed/TalentCard.jsx';
import { Loader } from 'semantic-ui-react';
import CompanyProfile from '../TalentFeed/CompanyProfile.jsx';
import FollowingSuggestion from '../TalentFeed/FollowingSuggestion.jsx';
import { BodyWrapper, loaderData } from '../Layout/BodyWrapper.jsx';

export default class TalentFeed extends React.Component {
    constructor(props) {
        super(props);

        let loader = loaderData
        loader.allowedUsers.push("Employer")
        loader.allowedUsers.push("Recruiter")

        this.state = {
            loadNumber: 5,
            loadPosition: 0,
            feedData: [],
            watchlist: [],
            loaderData: loader,
            loadingFeedData: false,
            companyDetails: null
        }

        this.init = this.init.bind(this);
        this.handleScroll = this.handleScroll.bind(this); 
        this.loadData = this.loadData.bind(this);
        this.renderCards = this.renderCards.bind(this);
    };

    init() {
        let loaderData = TalentUtil.deepCopy(this.state.loaderData)
        loaderData.isLoading = false;
        this.setState({ loaderData });//comment this
        this.loadData();
    }

    componentDidMount() {
        window.addEventListener('scroll', this.handleScroll);
        this.init();
    };

    handleScroll() {
        const win = $(window);
        if ((($(document).height() - win.height()) == Math.round(win.scrollTop())) || ($(document).height() - win.height()) - Math.round(win.scrollTop()) == 1) {
            this.setState({ loadPosition: this.state.loadPosition + this.state.loadNumber }, this.loadData)
            
        }
    };

    loadData() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://localhost:60290/profile/profile/getTalent?Position=' + this.state.loadPosition + '&Number=' + this.state.loadNumber,
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            success: function (res) {
                this.setState({ feedData: this.state.feedData.concat(res.data) })
                debugger;
            }.bind(this),
            error: function (res) {
                console.log(res.status)
            }
        })
    }

    renderCards() {
        const { feedData } = this.state;
        return (
            feedData.map(x => {
                return  <TalentCard
                    key={x.id}
                    feedData={x}
                />
            })
            )
    }

    render() {
        const { feedData } = this.state;
        
        return (
            <BodyWrapper reload={this.init} loaderData={this.state.loaderData}>
                <div className="ui grid talent-feed container">
                    <div className="four wide column">
                        <CompanyProfile />
                    </div>
                    <div className="eight wide column">
                        {feedData.length>0 ?
                            this.renderCards()
                            :
                            <p>There are no talens found for your recruitment company</p>
                        }
                        
                    </div>
                    <div className="four wide column">
                        <div className="ui card">
                            <FollowingSuggestion />
                        </div>
                    </div>
                </div>
            </BodyWrapper>
        )
    }
}

