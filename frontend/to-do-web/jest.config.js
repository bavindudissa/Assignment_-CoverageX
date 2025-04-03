module.exports = {
    moduleNameMapper: {
        "^@/(.*)$": "<rootDir>/src/$1",
      },
      setupFilesAfterEnv: ['<rootDir>/src/__tests__/setupTests.js'],
      testEnvironment: "jest-environment-jsdom",
      transform: {
        "^.+\\.[t|j]sx?$": "babel-jest",
      },
  };
  