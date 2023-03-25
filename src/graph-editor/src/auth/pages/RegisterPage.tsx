import React, { FC } from "react";
import PromoGraph from "graph/PromoGraph";
import styles from "graph/pages/MainPage.module.scss";
import { useNavigate } from "react-router-dom";
import { FormikProvider, useFormik } from "formik";
import { Button, Form, Spinner } from "react-bootstrap";
import * as yup from "yup";
import CenteredContainer from "components/CentredContainer";
import { RegisterUser, authApi } from "auth/authApi";
import FormInput from "../../components/forms/FormInput";




const registerUserValidationSchema = yup.object({
    userName: yup.string().required("Empty name"),
    email: yup.string().email("Not a valid email").required("Empty email"),
    password: yup.string().required("Empty password"),
});

const RegisterPage: FC = React.memo(() => {
    const navigate = useNavigate();
    const [registerUser, mutation] = authApi.useRegisterUserMutation();


    const formik = useFormik<RegisterUser>({
        initialValues: {
            userName: "",
            email: "",
            password: "",
        },

        validationSchema: registerUserValidationSchema,
        onSubmit: async (data, {
            setSubmitting,
            setFieldError,
            setStatus
        }) => {
            try {
                const payload = await registerUser(data).unwrap();
                console.log('fulfilled', payload);
                navigate('../login');
            } catch (error) {
                setFieldError('userName', 'Error registering user');
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
                            <FormInput field="email" placeholder="Email" />
                            <FormInput field="password" type="password" placeholder="Password" />
                            <Button
                                type="submit"
                                variant="light"
                                className="w-100 mt-3"
                                disabled={mutation.isLoading}
                            >
                                {mutation.isLoading
                                    ? <Spinner animation="border" />
                                    : <> Start using Graph Editor</>
                                }
                            </Button>
                        </Form>
                    </FormikProvider>
                </div>
            </main>
        </CenteredContainer>
    );
});
RegisterPage.displayName = "RegisterPage";
export default RegisterPage;