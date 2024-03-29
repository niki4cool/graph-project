﻿import React, { FC } from "react";
import PromoGraph from "graph/PromoGraph";
import styles from "graph/pages/MainPage.module.scss";
import { useNavigate } from "react-router-dom";
import { FormikProvider, useFormik } from "formik";
import { Button, Form } from "react-bootstrap";
import * as yup from "yup";
import CenteredContainer from "components/CentredContainer";
import FormInput from "../../components/forms/FormInput";
import { RegisterUser, userService } from "../authApi";




const registerUserValidationSchema = yup.object({
    userName: yup.string().required("Empty name"),
    password: yup.string().required("Empty password"),    
});

const RegisterPage: FC = React.memo(() => {
    const navigate = useNavigate();


    const formik = useFormik<RegisterUser>({
        initialValues: {
            userName: "",
            password: "",
        },

        validationSchema: registerUserValidationSchema,
        onSubmit: async (data, {
            setSubmitting,
            setFieldError,
            setStatus
        }) => {
            userService.register(data).then((value) => {
                navigate('../login')
            }).catch((reason) => {
                setFieldError('userName', 'Error ' + reason);
            });
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
                                <>Зарегистрироваться</>
                            </Button>
                        </Form>
                    </FormikProvider>
                </div>
                <div>
                    Уже есть аккаунт?
                </div>
                <a className=""

                    onClick={() => {
                        navigate("../login");
                    }}
                >
                    Вход
                </a>
            </main>
        </CenteredContainer>
    );
});
RegisterPage.displayName = "RegisterPage";
export default RegisterPage;