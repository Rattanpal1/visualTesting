var executeCommand = require("C:\\Users\\ranjeet.rattanpal\\source\\repos\\TestProject\\node_modules\\backstopjs\\core\\command\\index");
var makeConfig = require("C:\\Users\\ranjeet.rattanpal\\source\\repos\\TestProject\\node_modules\\backstopjs\\core\\util\\makeConfig");
module.exports = function (command, options) {
var config = makeConfig(command, options);
config.tempCompareConfigFileName = options.compareConfigFile;
return executeCommand(command, config);
};
