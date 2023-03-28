import React, { Component, FC } from "react";
import { authUser } from "../../auth/authApi";
import LoginButton from "./LoginButton";


class Username extends Component {
    render() {
        var user = authUser();
        if (user)
            return <h1>{user.name}</h1>;
        else
            <LoginButton />;
    }
}

export default Username;