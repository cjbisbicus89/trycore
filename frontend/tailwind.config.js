/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/**/*.{html,ts,css}",
  ],
  theme: {
    extend: {
      colors: {
        trycore: {
          navy: '#1A2B4A',
          blue: '#1E3A6E',
          orange: '#FF6B2B',
          'orange-light': '#FF8C55',
        }
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
      }
    },
  },
  plugins: [],
}
