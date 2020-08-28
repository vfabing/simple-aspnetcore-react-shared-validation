import React from 'react';
import { useForm } from 'react-hook-form';
import { ErrorMessage } from '@hookform/error-message';
import useValidation from '../custom-hooks/useValidation';

const Home: React.FunctionComponent<{}> = (props) => {
  const { register, handleSubmit, errors } = useForm();
  const onSubmit = (data: any) => console.log(data);
  const { validationRules } = useValidation('CreateUserDtoValidator');

  return (<div>
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="input__block">
        <label className="input__label" htmlFor="firstName">firstName:</label>
        <input id="firstName" name="firstName" ref={register(validationRules && validationRules["FirstName"])} />
        <ErrorMessage errors={errors} name={"firstName"} />
      </div>
      <div className="input__block">
        <label className="input__label" htmlFor="lastName">lastName:</label>
        <input name="lastName" ref={register(validationRules && validationRules["LastName"])} />
        <ErrorMessage errors={errors} name={"lastName"} />
      </div>
      <div className="input__block">
        <label className="input__label" htmlFor="email">email:</label>
        <input type="email" name="email" ref={register(validationRules && validationRules["Email"])} />
        <ErrorMessage errors={errors} name={"email"} />
      </div>
      <div className="input__block">
        <label className="input__label" htmlFor="age">age:</label>
        <input type="number" name="age" ref={register(validationRules && validationRules["Age"])} />
        <ErrorMessage errors={errors} name={"age"} />
      </div>
      <div className="input__block">
        <label className="input__label" htmlFor="birthDate">birthDate:</label>
        <input type="date" name="birthDate" ref={register(validationRules && validationRules["BirthDate"])} />
        <ErrorMessage errors={errors} name={"birthDate"} />
      </div>
      <input type="submit" />
    </form>
  </div>)
}

export default Home;