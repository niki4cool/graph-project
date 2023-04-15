import React, { FC } from "react";
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
    email: yup.string().email("Not a valid email").required("Empty email"),
    password: yup.string().required("Empty password"),
});

const RegisterPage: FC = React.memo(() => {
    const navigate = useNavigate();


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
                            >
                                <> Start using Graph Editor</>
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