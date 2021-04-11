﻿import React, { Component } from 'react';
import { FacebookProvider, ShareButton } from 'react-facebook';
import { Telegram, Twitter, Linkedin } from 'react-social-sharing';
import config from '../../../config';
import './share.css';

export default class ShareButtons extends Component {
    render() {
        return (
            <>
                <FacebookProvider appId={config.FACEBOOK_CLIENT_ID}>
                    <ShareButton className="btn" href={this.props.href} >
                        <div id="fb-share-button">
                            <i className="fab fa-facebook text-white"></i>
                        </div>
                    </ShareButton>
                </FacebookProvider>

                <Telegram solid small link={this.props.href} />
                <Twitter solid small link={this.props.href} />
                <Linkedin solid small link={this.props.href} />
            </>
        );
    }
}
