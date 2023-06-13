import React, { FC, useEffect, useState } from "react";
import { Button, ButtonGroup, Dropdown, DropdownButton, FloatingLabel, Form, Spinner } from "react-bootstrap";
import { FormikProvider, useFormik } from "formik";
import styles from "graph/pages/MainPage.module.scss";
import PromoGraph from "graph/PromoGraph";
import CenteredContainer from "components/CentredContainer";
import { useNavigate } from "react-router-dom";
import FormInput from "components/forms/FormInput";
import * as yup from "yup";
import { graphsApi } from "graph/graphsApi";
import { Graph } from "../graphDataSlice";


export interface GraphConnectFromProps {
    graphId: string;
}

const graphConnectValidationSchema = yup.object({
    graphId: yup.string().required("Enter graph name")
});

const GraphCreationPage: FC = React.memo(() => {
    const navigate = useNavigate();
    const graphs = graphsApi.useGetGraphsQuery();
    const [putGraph, { isLoading, isSuccess, isError, error }] = graphsApi.usePutGraphMutation();
    const [graphType, setGraphType] = useState<string>("Граф классов");
    const [classGraph, setClassGraph] = useState<string>("");

    useEffect(() => {
        if (error) {
            navigate("../login");
        }
    }, [isSuccess, error]);


    const formik = useFormik<GraphConnectFromProps>({
        initialValues: { graphId: "" },
        validationSchema: graphConnectValidationSchema,
        onSubmit: data => {
            let request =
            {
                id: data.graphId,
                type: graphType,
                classId: classGraph
            };
            putGraph(request).then(() => {
                navigate("/graph/" + data.graphId);
            });
        }
    });


    return (
        <CenteredContainer>
            <PromoGraph />
            <main className={styles.main}>
                <h1>Graph creation</h1>
                <FormikProvider value={formik}>
                    <Form noValidate onSubmit={formik.handleSubmit}>
                        <FormInput field="graphId" placeholder="Имя графа" />
                        
                        <ButtonGroup>
                            <Button variant={graphType === "ClassGraph" ? "primary" : "light"} className="w-100 mt-3" onClick={() => setGraphType("ClassGraph")}>Граф классов</Button>
                            <Button variant={graphType === "InstanceGraph" ? "primary" : "light"} className="w-100 mt-3" onClick={() => setGraphType("InstanceGraph")}>Граф сущностей</Button>
                        </ButtonGroup>
                        <br />

                        {graphType === "ClassGraph" ? <></> :
                            graphs.data == undefined ? <h4>Загрузка</h4> : <>

                                <Dropdown >
                                    <Dropdown.Toggle variant="light" className="w-100 mt-3">
                                        {classGraph === "" ? "Выберите граф классов" : classGraph}
                                    </Dropdown.Toggle>
                                    <Dropdown.Menu>
                                        {(graphs.currentData?.filter(g => g.graphType == "ClassGraph") as Graph[]).map(g =>
                                            <Dropdown.Item onClick={() => setClassGraph(g.name)}>{g.name}</Dropdown.Item>)}
                                    </Dropdown.Menu>
                                </Dropdown>
                            </>
                        }

                        <Button
                            type="submit"
                            variant="light"
                            className="w-100 mt-3"
                        >
                            <>Создать</>
                        </Button>
                    </Form>
                </FormikProvider>
            </main>
        </CenteredContainer >
    );
});

GraphCreationPage.displayName = "GraphCreationPage";
export default GraphCreationPage;