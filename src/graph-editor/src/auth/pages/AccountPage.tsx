import React, { FC, useEffect, useState } from "react";

import List from "components/lists/List";
import Sidebar from "components/Sidebar";
import ListItem from "components/lists/ListItem";
import { graphsApi } from "graph/graphsApi";

const AccountPage: FC = React.memo(() => {
    const graphs = graphsApi.useGetGraphsQuery();    

    if (graphs.data == undefined)
        return <></>;

    return <Sidebar title="Graphs">
        <List
            header="Graphs"
            emptyHeader="No graphs"
            items={graphs.currentData as string[]}
            renderItem={graph =>
                <ListItem
                    children={<h4>{graph}</h4>}
                    key={graph}
                />
            }
        />
    </Sidebar>
});

AccountPage.displayName = "AccountPage";
export default AccountPage;