import {Form, FormControl, FormControlProps} from "react-bootstrap";
import React, {FC} from "react";
import {useField} from "formik";

interface FormInputProps extends FormControlProps {
  field: string;
}

const FormInput: FC<FormInputProps> =
  React.memo(({field, ...props}) => {
    const [fieldProps, meta] = useField(field);
    return (
      <>
        <FormControl
          {...fieldProps}
          {...props}
          isValid={meta.touched && !meta.error}
          isInvalid={meta.touched && !!meta.error}
        />
        <Form.Control.Feedback type="invalid" tooltip>
          {meta.error}
        </Form.Control.Feedback>
      </>
    );
  });
FormInput.displayName = "FormInput";
export default FormInput;