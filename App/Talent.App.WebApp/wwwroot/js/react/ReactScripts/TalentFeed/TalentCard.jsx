import React from 'react';
import ReactPlayer from 'react-player';
import PropTypes from 'prop-types'
import { Button, Image, Icon, Card, Grid } from 'semantic-ui-react'

export default class TalentCard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props)
        const feedData = props.feedData ?
            Object.assign({}, props.feedData)
            : {
                currentEmployment: '',
                level: '',
                name: '',
                photoId: '',
                skills: [],
                summary: '',
                visa: '',
                videoUrl: '',
                cVUrl: ''
            }
        this.state = {
            feedData: feedData,
            profileDisplay: false
        }
        
        this.changeStatus = this.changeStatus.bind(this);
    };

    componentWillReceiveProps(nextProps) {
        console.log(nextProps)
        this.setState({
            feedData: Object.assign({}, nextProps.feedData),
        });
    }

    changeStatus() {
        const { profileDisplay } = this.state;
        this.setState({ profileDisplay: !profileDisplay})
    }

    render() {
        const { feedData, profileDisplay } = this.state;
        feedData.videoUrl = feedData.videoUrl ? feedData.videoUrl : null;
        feedData.photoId = feedData.photoId ? feedData.photoId : "http://localhost:60290/images/Capture.PNG";
        return (
            <Card className='talent-card'>
                <Card.Content>
                    <Card.Header>
                        <Grid columns={2} width='8'>
                            <Grid.Column>{feedData.name}</Grid.Column>
                            <Grid.Column textAlign='right'><Icon name='star' /></Grid.Column>
                        </Grid>
                    </Card.Header>
                    
                    <div height='250px' className='talent-card img'>
                    {!profileDisplay ?
                        (feedData.videoUrl != null ? <video className='image' controls src={feedData.videoUrl} />
                                : <div className='divFor' />)
                        :
                        <Card.Description>
                            <Grid columns={2}>
                                <Grid.Column><Image src={feedData.photoId} /></Grid.Column>
                                <Grid.Column>
                                    <h5>Current employer</h5>
                                    <p>{feedData.currentEmployment}</p>
                                    <h5>Level</h5>
                                    <p>{feedData.level}</p>
                                    
                                    <h5>Visa status</h5>
                                    <p>{feedData.visa}</p>
                                    <h5>Summary</h5>
                                    <p>{feedData.summary}</p>

                                </Grid.Column>
                            </Grid>
                        </Card.Description>
                        }
                    </div>
                </Card.Content>
                <Card.Content>
                    <Grid columns={4}>
                        <Grid.Column>
                            {!profileDisplay ? <Icon name='user' size='large' onClick={this.changeStatus} />
                                : <Icon name='video' size='large' onClick={this.changeStatus}/>}
                        </Grid.Column>
                        <Grid.Column><Icon name='file pdf outline' size='large' /></Grid.Column>
                        <Grid.Column><Icon name='linkedin' size='large' /></Grid.Column>
                        <Grid.Column><Icon name='github' size='large' /></Grid.Column>
                    </Grid>
                    
                </Card.Content>
                <Card.Content extra>
                    {(feedData.skills != null) ?
                        feedData.skills.map(x =>
                            <Button type='button' basic color='blue' key={x}>{x}</Button>
                        )
                        :
                        null}
                </Card.Content>
            </Card>
        )
    }
}

