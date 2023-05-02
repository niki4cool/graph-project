import { Form, FormControlProps, InputGroup } from "react-bootstrap";
import React from "react";
import { useField } from "formik";

interface FormInputProps extends FormControlProps {
    field: string;
    children?: React.ReactNode;
}

const FormInlineInput =
    React.forwardRef<HTMLInputElement, FormInputProps>(({ field, children, ...props }, ref) => {
        const [fieldProps, meta] = useField(field);
        return (
            <>
                <InputGroup>
                    <Form.Control
                        ref={ref}
                        {...fieldProps}
                        {...props}
                        isValid={meta.touched && !meta.error}
                        isInvalid={meta.touched && !!meta.error}
                    />
                    <Form.Control.Feedback
                        type="invalid"
                        tooltip
                        style={
                            { display: meta.touched && !!meta.error ? "block" : "none" }
                        }>
                        {meta.error}
                    </Form.Control.Feedback>
                    {children}
                </InputGroup>
            </>
        );
    });
FormInlineInput.displayName = "FormInlineInput";
export default FormInlineInput;