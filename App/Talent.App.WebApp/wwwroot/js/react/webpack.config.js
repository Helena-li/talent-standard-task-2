
module.exports = {
    context: __dirname,
    entry: {
        homePage: './ReactScripts/Home.js'
    },
    output:
    {
        path: __dirname + "/dist",
        filename: "[name].bundle.js"
    },
    watch: true,
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['babel-preset-env', 'babel-preset-react']
                    }
                }
            },
            {
                test: /\.css$/,
                loaders: [
                    'style-loader',
                    'css-loader?modules'
                ]
            }
        ]
    }
}