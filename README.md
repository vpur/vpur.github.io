# vpur.github.io [![Build status][build status]][appveyor]

[VPur homepage][homepage]

## Build for production

Run: `./fake.sh build`

All the files needed for deployment are under the `output` folder.

## Watch mode

Run: `./fake.sh build -t Watch`

## Running Fable without FAKE

- Install NPM dependencies: `yarn`
- Install Nuget dependencies: `dotnet restore build.proj`
- Building for development: `dotnet fable webpack-dev-server`
- Building for production: `dotnet fable webpack-cli`

## Debugging in VS Code

- Install [Debugger For Chrome][vscode debugger for chrome] in vscode
- Press `F5` in vscode
- After all the `.fs` files are compiled, the browser will be launched
- Set a breakpoint in F#
- Either press `F5` in Chrome or restart debugging in VS Code with `Ctrl+Shift+F5` (`Cmd+Shift+F5` on macOS)
- The breakpoint will be caught in vscode

[homepage]: http://vpur.github.io/
[build status]: https://ci.appveyor.com/api/projects/status/4pqasc19yydecytb?svg=true
[appveyor]: https://ci.appveyor.com/project/VasylPurchel/vpur-github-io
[vscode debugger for chrome]: https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome
