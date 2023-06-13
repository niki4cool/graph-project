import React, { FC } from "react";
import HelpMessage from "components/HelpMessage";

export interface AddLinkHelpMessageProps {
    visible?: boolean;
    onCancel?: () => void;
}

const AddLinkHelpMessage: FC<AddLinkHelpMessageProps> = React.memo(({ visible, onCancel }) => {
    return (
        <HelpMessage visible={visible} cancelButton onCancel={onCancel}>
            Нажмите на вторую вершину для создания ребра.
        </HelpMessage>
    );
});
AddLinkHelpMessage.displayName = "AddLinkHelpMessage";
export default AddLinkHelpMessage;