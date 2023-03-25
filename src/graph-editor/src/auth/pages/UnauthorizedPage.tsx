import React, { FC, useEffect } from "react";
import { Button, FloatingLabel, Form, Spinner } from "react-bootstrap";
import { FormikProvider, useFormik } from "formik";
import styles from "graph/pages/MainPage.module.scss";
import PromoGraph from "graph/PromoGraph";
import CenteredContainer from "components/CentredContainer";
import { useNavigate } from "react-router-dom";
import FormInput from "components/forms/FormInput";
import * as yup from "yup";
import { graphsApi } from "graph/graphsApi";



export interface GraphConnectFromProps {
    graphId: string;
}

const UnauthorizedPage: FC = React.memo(() => {
    const navigate = useNavigate();

    const [putGraph, mutation] = graphsApi.usePutGraphMutation();

    useEffect(() => {
        if (mutation.isSuccess)
            navigate(`graphs/${mutation.originalArgs}`);
    }, [mutation.isSuccess, mutation.originalArgs, navigate]);

    return (
        <CenteredContainer>
            <PromoGraph />

            <main className={styles.main}>
                <style type="text/css">
                    position: absolute;

                    background: #37396A;
                    filter: blur(150px);
                </style>


                <h1 className={styles.header}>Graph Editor</h1>
                <p>Create and edit graphs online together with you team.</p>
                <div className={styles.forms}>
                    <Button
                        variant="outline-light"
                        className="w-100 mt-3"
                        disabled={mutation.isLoading}
                        onClick={() => { navigate("/login") }}
                    >
                        {<>Login</>}
                    </Button>

                    <Button
                        variant="light"
                        className="w-100 mt-3"
                        disabled={mutation.isLoading}
                        onClick={() => { navigate("/register") }}
                    >
                        {<>Register</>}
                    </Button>
                </div>
            </main>
        </CenteredContainer>
    );
});
UnauthorizedPage.displayName = "UnauthorizedPage";
export default UnauthorizedPage;