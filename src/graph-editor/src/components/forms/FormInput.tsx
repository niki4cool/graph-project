import {Form, FormControl, FormControlProps} from "react-bootstrap";
import React from "react";
import {useField} from "formik";

interface FormInputProps extends FormControlProps {
  field: string;
  noValidate?: boolean;
}

const FormInput = React.forwardRef<HTMLInputElement, FormInputProps>
(({field, noValidate, ...props}, ref) => {
  const [fieldProps, meta] = useField(field);
  return (
    <>
      <FormControl
        ref={ref}
        {...fieldProps}
        {...props}
        isValid={!noValidate && meta.touched && !meta.error}
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