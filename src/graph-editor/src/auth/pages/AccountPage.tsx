﻿import React, { FC, useEffect, useState } from "react";

import List from "components/lists/List";
import ListItem from "components/lists/ListItem";
import { graphsApi } from "graph/graphsApi";
import { Graph } from "../../graph/graphDataSlice";
import CenteredContainer from "../../components/CentredContainer";
import { useNavigate } from "react-router-dom";
import PromoGraph from "../../graph/PromoGraph";
import styles from "graph/pages/MainPage.module.scss";
import { Button } from "react-bootstrap";
import Username from "../../components/auth/Username";
import { isFetchBaseQueryError } from "../../rtkQuery";
import Toolbar from "../../components/toolbar/Toolbar";
import ToolbarDropdown from "../../components/toolbar/ToolbarDropdown";
import ToolbarItem from "../../components/toolbar/ToolbarItem";

const AccountPage: FC = React.memo(() => {
    const graphs = graphsApi.useGetGraphsQuery();
    const [deleteGraph, mutation] = graphsApi.useDeleteGraphMutation();
    const navigate = useNavigate();

    useEffect(() => {
        if (mutation.isSuccess)
            graphs.refetch();
    }, [mutation]);

    useEffect(() => {
        if (graphs.isError)
            console.log(graphs.error);
        if (isFetchBaseQueryError(graphs.error) && graphs.error.status === 401)
            navigate("/login");
    }, [graphs]);


    //Regular, ClassGraph, Free
    const freeGraphs = graphs.currentData?.filter(g => g.graphType === "Free") as Graph[];
    const instanceGraphs = graphs.currentData?.filter(g => g.graphType === "InstanceGraph") as Graph[];
    const classGraphs = graphs.currentData?.filter(g => g.graphType === "ClassGraph") as Graph[];


    const accountPage = () => {
        if (graphs.data === undefined)
            return <div>No data</div>;

        return <main className={styles.main}>
            <h2>Добро пожаловать, <Username /></h2>
            <List
                className="w-100 mt-3"
                header="Графы классов"
                emptyHeader="Нет графов классов"
                items={classGraphs}
                renderItem={graph =>
                    <ListItem
                        children={<div>{graph.name}</div>}
                        key={graph.id}
                        onChildrenClick={() => navigate("/graphs/" + graph.name)}
                        onRemoveClick={() => {
                            deleteGraph(graph.name);
                            navigate("/account");
                        }}
                    />
                }
            />
            <List
                className="w-100 mt-3"
                header="Графы сущностей"
                emptyHeader="Нет графов сущностей"
                items={instanceGraphs}
                renderItem={graph =>
                    <ListItem
                        children={<div>{graph.name}</div>}
                        key={graph.id}
                        onChildrenClick={() => navigate("/graphs/" + graph.name)}
                        onRemoveClick={() => {
                            deleteGraph(graph.name);
                            navigate("/account");
                        }}
                    />
                }
            />
            <Button
                variant="light"
                className="w-100 mt-3"
                onClick={() => navigate("/createGraph")}
            >
                <>Создать</>
            </Button>
        </main>
    }

    return <div>
        <Toolbar>
            <ToolbarItem onClick={() => navigate("../")}><h1 className={styles.header}>Graph Editor</h1></ToolbarItem>
        </Toolbar>
        <CenteredContainer>
            <PromoGraph />
            {accountPage()}
        </CenteredContainer>
    </div>
});

AccountPage.displayName = "AccountPage";
export default AccountPage;