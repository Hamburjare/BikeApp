import RingLoader from "react-spinners/RingLoader";

export default function Loading() {
  return (
    <div className="flex justify-center mt-10">
      <div className="text-white text-center text-3xl mt-3">
        Loading...
        <RingLoader color={"#ffffff"} loading={true} size={150} />
      </div>
    </div>
  );
}
