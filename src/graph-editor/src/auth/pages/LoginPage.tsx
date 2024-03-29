﻿import React, { FC } from "react";
import PromoGraph from "graph/PromoGraph";
import styles from "graph/pages/MainPage.module.scss";

import { Button, Form } from "react-bootstrap";
import CenteredContainer from "components/CentredContainer";
import { LoginUser, userService } from "auth/authApi";
import { FormikProvider, useFormik } from "formik";
import * as yup from "yup";
import FormInput from "../../components/forms/FormInput";
import { useNavigate } from "react-router-dom";


const loginUserValidationSchema = yup.object({
    userName: yup.string().required("Empty name"),
    password: yup.string().required("Empty password"),
});


const LoginPage: FC = React.memo(() => {
    const navigate = useNavigate();

    const formik = useFormik<LoginUser>({
        initialValues: {
            userName: "",
            password: "",
        },

        validationSchema: loginUserValidationSchema,
        onSubmit: async (data, {
            setSubmitting,
            setFieldError,
            setStatus
        }) => {
            userService.login(data).then(result => {
                navigate("../");
            }).catch((reason) => {
                setFieldError('userName', 'Error ' + reason);
            })
        }
    });

    return (
        <CenteredContainer>
            <PromoGraph />
            <main className={styles.main}>
                <h1>Graph Editor</h1>
                <br />
                <div className={styles.forms}>
                    <FormikProvider value={formik}>
                        <Form noValidate onSubmit={formik.handleSubmit}>
                            <FormInput field="userName" placeholder="Логин" />
                            <br />
                            <FormInput field="password" type="password" placeholder="Пароль" />
                            <br />
                            <Button
                                type="submit"
                                variant="light"
                                className="w-100 mt-3"
                            >
                                <>Войти</>
                            </Button>
                        </Form>
                    </FormikProvider>
                </div>
                <div>
                    Нет аккаунта?
                </div>
                <a className=""

                    onClick={() => {
                        navigate("../register");
                    }}
                >
                    Регистрация
                </a>
            </main>
        </CenteredContainer>
    );
});
LoginPage.displayName = "LoginPage";
export default LoginPage;