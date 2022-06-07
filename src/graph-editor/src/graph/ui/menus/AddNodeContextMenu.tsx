import React, {FC, useEffect, useRef} from "react";
import {Button, Form} from "react-bootstrap";
import {FormikProvider, useFormik} from "formik";
import FormInlineInput from "components/forms/FormInlineInput";
import styles from "graph/ui/menus/AddNodeContextMenu.module.scss";
import * as yup from "yup";
import ContextMenu, {ContextMenuProps} from "components/ContextMenu";
import {useAppSelector} from "store";
import {graphDataSelector} from "graph/graphDataSlice";

export interface AddNodeFormData {
  id: string;
}

export const addNodeValidationSchema = yup.object({
  id: yup.string().label("Id").required()
});

export interface AddNodeContextMenuProps extends ContextMenuProps {
  onAdd?: (data: AddNodeFormData) => void;
}

const AddNodeContextMenu: FC<AddNodeContextMenuProps> =
  React.memo(({onAdd, ...props}) => {
    const graphData = useAppSelector(graphDataSelector);
    const inputRef = useRef<HTMLInputElement>(null);

    const formik = useFormik<AddNodeFormData>({
      initialValues: {id: ""},
      validationSchema: addNodeValidationSchema,
      validate: data => {
        const isIdUnique = graphData.nodes.findIndex(x => x.id === data.id)! < 0;
        if (!isIdUnique)
          return {id: "Node with such id already exist"};
      },
      onSubmit: data => {
        onAdd && onAdd(data);
        formik.resetForm();
      }
    });

    useEffect(() => {
      if (props.position)
        inputRef.current?.focus();
    }, [props.position]);

    return (
      <ContextMenu {...props} onOutsideClick={() => {
        props.onOutsideClick && props.onOutsideClick();
        formik.resetForm();
      }}>
        <FormikProvider value={formik}>
          <Form
            noValidate
            onSubmit={formik.handleSubmit}
          >
            <FormInlineInput field="id" placeholder="Id" className={styles.input} ref={inputRef}>
              <Button type="submit" variant="light">Add node</Button>
            </FormInlineInput>
          </Form>
        </FormikProvider>
      </ContextMenu>
    );
  });
AddNodeContextMenu.displayName = "AddNodeContextMenu";
export default AddNodeContextMenu;