import React, {FC} from "react";
import HelpMessage from "components/HelpMessage";

export interface AddLinkHelpMessageProps {
  visible?: boolean;
  onCancel?: () => void;
}

const AddLinkHelpMessage: FC<AddLinkHelpMessageProps> = React.memo(({visible, onCancel}) => {
  return (
    <HelpMessage visible={visible} cancelButton onCancel={onCancel}>
      Click on target node
    </HelpMessage>
  );
});
AddLinkHelpMessage.displayName = "AddLinkHelpMessage";
export default AddLinkHelpMessage;