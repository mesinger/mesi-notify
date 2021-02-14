const path = require('path');

module.exports = {
  entry: {
    main: ['./src/scripts/main.ts', './src/styles/main.scss']
  },
  mode: "production",
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
      {
        test: /\.s[ac]ss$/i,
        use: [
          {
            loader: 'file-loader',
            options: {
              name: '[name].css',
              outputPath: 'styles'
            }
          },
          {
            loader: 'extract-loader'
          },
          {
            loader: 'css-loader'
          },
          {
            loader: 'sass-loader'
          },
        ]
      }
    ],
  },
  resolve: {
    extensions: ['.tsx', '.ts', '.js'],
  },
  output: {
    filename: 'scripts/main.js',
    path: path.resolve(__dirname, '../wwwroot'),
  },
};
