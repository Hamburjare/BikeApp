import { useRouteError } from "react-router-dom";
import { Link } from "react-router-dom";

export default function ErrorPage() {
  const error = useRouteError();
  console.error(error);

  return (
    <div class="flex items-center justify-center h-screen bg-white">
      <div>
        <h1 class="text-center text-5xl mb-2 text-blacktext" >Oops!</h1>
        <p class="text-center text-3xl mb-2 text-blacktext" >Sorry, an unexpected error has occurred.</p>
        <p class="text-center text-2xl mb-2 text-blacktext" >
          <i>{error.statusText || error.message}</i>
        </p>
        <Link to="/" className="flex justify-center">
          <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
            Go Home
          </button>
        </Link>
      </div>
      
    </div>
  );
}