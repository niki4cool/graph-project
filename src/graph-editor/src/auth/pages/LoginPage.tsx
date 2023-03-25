import React, { FC } from "react";
import PromoGraph from "graph/PromoGraph";
import styles from "graph/pages/MainPage.module.scss";
import { useNavigate } from "react-router-dom";
import { Button, Form } from "react-bootstrap";
import CenteredContainer from "components/CentredContainer";
import { LoginUser, authApi } from "auth/authApi";
import userDataSlice from "auth/authSlice";
import { UserData } from "auth/authSlice";
import { FormikProvider, useFormik } from "formik";
import * as yup from "yup";
import FormInput from "../../components/forms/FormInput";


const loginUserValidationSchema = yup.object({
    userName: yup.string().required("Empty name"),
    password: yup.string().required("Empty password"),
});




const LoginPage: FC = React.memo(() => {
    const navigate = useNavigate();
    const [loginUser, mutation] = authApi.useLoginUserMutation();

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

            try {
                const payload = await loginUser(data).unwrap();
                console.log('fulfilled', payload);
                userDataSlice.actions.login({
                    userName: data.userName,
                    token: payload.jwt,
                } as UserData);
                navigate('../main');
            } catch (error) {
                setFieldError('userName', 'Error');
            }
        }
    });

    return (
        <CenteredContainer>
            <PromoGraph />
            <main className={styles.main}>
                <h1>Graph Editor</h1>
                <div className={styles.forms}>
                    <FormikProvider value={formik}>
                        <Form noValidate onSubmit={formik.handleSubmit}>
                            <FormInput field="userName" placeholder="Username" />
                            <FormInput field="password" type="password" placeholder="Password" />
                            <Button
                                type="submit"
                                variant="light"
                                className="w-100 mt-3"
                            >
                            </Button>
                        </Form>
                    </FormikProvider>
                </div>
            </main>
        </CenteredContainer>
    );
});
LoginPage.displayName = "LoginPage";
export default LoginPage;