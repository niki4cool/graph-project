import React, { FC, useEffect } from "react";
import { Button, FloatingLabel, Form, Spinner } from "react-bootstrap";
import { FormikProvider, useFormik } from "formik";
import styles from "graph/pages/MainPage.module.scss";
import PromoGraph from "graph/PromoGraph";
import CenteredContainer from "components/CentredContainer";
import { useNavigate } from "react-router-dom";
import FormInput from "components/forms/FormInput";
import * as yup from "yup";
import Toolbar from "../../components/toolbar/Toolbar";
import Username from "../../components/auth/Username";

export interface GraphConnectFromProps {
    graphId: string;
}

const graphConnectValidationSchema = yup.object({
    graphId: yup.string().required("Select graph name to get started")
});


const MainPage = () => {
    const navigate = useNavigate();



    const formik = useFormik<GraphConnectFromProps>({
        initialValues: { graphId: "" },
        validationSchema: graphConnectValidationSchema,
        onSubmit: async (data, {
            setSubmitting,
            setFieldError,
            setStatus
        }) => {
            navigate(`../graphs/${data.graphId}`);
        }
    });

    return (

        <CenteredContainer>
                
            <PromoGraph />
            <main className={styles.main}>
                <h1 className={styles.header}>Graph Editor</h1>
                <h2>Welcome, <Username /></h2>
                <p>Create and edit graphs online together with you team.</p>
                <div className={styles.forms}>
                    <FormikProvider value={formik}>
                        <Form noValidate onSubmit={formik.handleSubmit}>
                            <FormInput field="graphId" placeholder="Graph name" />
                            <Button
                                type="submit"
                                variant="light"
                                className="w-100 mt-3"
                            >
                                <> Start using Graph Editor</>
                            </Button>
                        </Form>
                    </FormikProvider>
                </div>
            </main>
        </CenteredContainer>
    );
}

MainPage.displayName = "MainPage";
export default MainPage;
