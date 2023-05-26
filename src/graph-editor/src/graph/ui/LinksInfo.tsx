import React, { FC } from "react";
import { useAppDispatch, useAppSelector } from "store";
import graphDataSlice, {
    asSourceLinksSelector,
    asTargetLinksSelector,
    GraphLink,
} from "graph/graphDataSlice";
import List from "components/lists/List";
import ListItem from "components/lists/ListItem";
import NodeLongName from "graph/ui/menus/NodeLongName";

export interface LinksInfoProps {
    nodeId: string;
    onNodeClick?: (nodeId: string) => void;
    onRemoveClick: (linkId: string) => void;
}

const LinksInfo: FC<LinksInfoProps> = React.memo(({ nodeId, onNodeClick, onRemoveClick }) => {
    const asSourceLinks = useAppSelector(asSourceLinksSelector(nodeId));
    const asTargetLinks = useAppSelector(asTargetLinksSelector(nodeId));

    const renderLink = (link: GraphLink, nodeId: string) =>
        <ListItem
            children={<NodeLongName nodeId={nodeId} />}
            onRemoveClick={() => onRemoveClick && onRemoveClick(link.id)}
            onChildrenClick={() => onNodeClick && onNodeClick(nodeId)}
            key={link.target + link.source}
        />;

    return (
        <>
            <List
                items={asSourceLinks}
                header="Points to"
                emptyHeader="Doesn't point no any nodes"
                className="mb-3"
                renderItem={link => renderLink(link, link.target)}
            />

            <List
                items={asTargetLinks}
                header="Points from"
                emptyHeader="No nodes points to it"
                renderItem={link => renderLink(link, link.source)}
            />
        </>
    );
});
LinksInfo.displayName = "LinksList";
export default LinksInfo;