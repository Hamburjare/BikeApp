/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        "primary": "#06191D",
        "secondary": "#00C5C8",
        "tertiary": "#70E7D1",
        "dusty-rose": "#E9DDD4",
        "blacktext": "#1d1d1f",
        "white": "#fbfbfd",
      }
    },
  },
  plugins: [],
}