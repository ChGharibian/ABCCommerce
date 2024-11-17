import { useRouteError } from "react-router-dom";

export default function ErrorPage() {
  const error = useRouteError();
  console.error(error);

  return (
    <div id="error-page">
      <h1>How did you get here?</h1>
      <p>A unexpected error has occured. You tried to route to a path that does not exist</p>
      <p>
        <i>{error.statusText || error.message}</i>
      </p>
    </div>
  );
}