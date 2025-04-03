import '@testing-library/jest-dom';

globalThis.import = {
    meta: {
      env: {
        VITE_API_URL: 'http://localhost:5133/api'
      }
    }
  };

test('dummy test to satisfy Jest', () => {});
