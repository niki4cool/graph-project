import React, { Component, FC } from "react";
import { Button, } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

function LoginButton() {
        const navigate = useNavigate();

        const redirectToLogin = () => {
            navigate("../login");
        };

        return <Button
            type="submit"
            variant="light"
            className="w-100 mt-3"
            onClick={redirectToLogin}
        >
            <>Login</>
        </Button>;
}

export default LoginButton;