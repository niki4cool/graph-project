import React, {FC, useEffect} from "react";
import {Button, FloatingLabel, Form, Spinner} from "react-bootstrap";
import {FormikProvider, useFormik} from "formik";
import styles from "graph/pages/MainPage.module.scss";
import PromoGraph from "graph/PromoGraph";
import CenteredContainer from "components/CentredContainer";
import {useNavigate} from "react-router-dom";
import FormInput from "components/forms/FormInput";
import * as yup from "yup";
import {graphsApi} from "graph/graphsApi";

export interface GraphConnectFromProps {
  graphId: string;
}

const graphConnectValidationSchema = yup.object({
  graphId: yup.string().required("Select graph name to get started")
});

const MainPage: FC = React.memo(() => {
  const navigate = useNavigate();
  const [putGraph, mutation] = graphsApi.usePutGraphMutation();

  const formik = useFormik<GraphConnectFromProps>({
    initialValues: {graphId: ""},
    validationSchema: graphConnectValidationSchema,
    onSubmit: data => {
      putGraph(data.graphId);
    }
  });

  useEffect(() => {
    if (mutation.isSuccess)
      navigate(`graphs/${mutation.originalArgs}`);
  }, [mutation.isSuccess, mutation.originalArgs, navigate]);

  return (
    <CenteredContainer>
      <PromoGraph/>
      <main className={styles.main}>
        <h1 className={styles.header}>Graph Editor</h1>
        <p>Create and edit graphs online together with you team.</p>
        <div className={styles.forms}>
          <FormikProvider value={formik}>
            <Form noValidate onSubmit={formik.handleSubmit}>
                <FormInput field="graphId" placeholder="Graph name"/>
              <Button
                type="submit"
                variant="light"
                className="w-100 mt-3"
                disabled={mutation.isLoading}
              >
                {mutation.isLoading
                  ? <Spinner animation="border"/>
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
MainPage.displayName = "MainPage";
export default MainPage;
