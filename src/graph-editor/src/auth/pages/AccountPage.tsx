import React, { FC, useEffect, useState } from "react";

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

const AccountPage: FC = React.memo(() => {
    const graphs = graphsApi.useGetGraphsQuery();
    const [deleteGraph, mutation] = graphsApi.useDeleteGraphMutation();
    const navigate = useNavigate();

    useEffect(() => {
        if (mutation.isSuccess)
            graphs.refetch();
    }, [mutation]);

    if (graphs.data == undefined)
        return <div>No data</div>;
    //Regular, ClassGraph, Free
    const freeGraphs = graphs.currentData?.filter(g => g.graphType == "Free") as Graph[];
    const instanceGraphs = graphs.currentData?.filter(g => g.graphType == "InstanceGraph") as Graph[];
    const classGraphs = graphs.currentData?.filter(g => g.graphType == "ClassGraph") as Graph[];



    return <CenteredContainer>
        <PromoGraph />
        <main className={styles.main}>
            <h1 className={styles.header}>Graph Editor</h1>
            <h2>Welcome, <Username /></h2>
            <List
                className="w-100 mt-3"
                header="Class graphs"
                emptyHeader="No class graphs"
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
                header="Instance graphs"
                emptyHeader="No instance graphs"
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
                <>Create new</>
            </Button>
        </main>
    </CenteredContainer>
});

AccountPage.displayName = "AccountPage";
export default AccountPage;