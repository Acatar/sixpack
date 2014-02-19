var sixpack = (function () {
    "use strict";

    var utils,
        exceptions,
        models,
        contexts,
        $sixpack;

    // utitlities based on jQuery
    utils = (function () {
        var $this = {},
            _objProto = Object.prototype,
            _objProtoToStringFunc = _objProto.toString,
            _objProtoHasOwnFunc = _objProto.hasOwnProperty,
            _class2Types = {},
            _class2ObjTypes = ["Boolean", "Number", "String", "Function", "Array", "Date", "RegExp", "Object", "Error"];

        for (var i in _class2ObjTypes) {
            var name = _class2ObjTypes[i];
            _class2Types["[object " + name + "]"] = name.toLowerCase();
        }

        $this.enforceLeadingSlash = function (str) {
            return str.substring(0, 1) == '/' ? str : '/' + str;
        }

        $this.removeLeadingSlash = function (str) {
            while (str.substring(0, 1) == '/') {
                return removeLeadingSlash(str.substring(1));
            }

            return str;
        }

        $this.enforceTrailingSlash = function (str) {
            return str.substring(str.length - 1, str.length) == '/' ? str : str + '/';
        }

        $this.arrayToCommaString = function (array) {
            if (array == null || array.length == 0)
                return null;

            var _output = '';

            for (var i in array) {
                var _item = array[i];

                if ($this.notNullOrWhitespace(_item))
                    _output += _item + ',';
            }

            return _output.substring(0, _output.length - 1);
        }

        $this.isWindow = function(obj) {
            return obj != null && obj == obj.window;
        };

        $this.type = function (obj) {
            if (typeof (obj) === "undefined")
                return "undefined";
            if (obj === null)
                return String(obj);

            return typeof obj === "object" || typeof obj === "function" ?
                _class2Types[_objProtoToStringFunc.call(obj)] || "object" :
                typeof obj;
        };

        $this.notDefined = function (obj) {
            try {
                return $this.type(obj) === 'undefined';
            }
            catch (e) {
                return true;
            }
        };

        $this.isDefined = function (obj) {
            try {
                return $this.type(obj) !== 'undefined';
            }
            catch (e) {
                return false;
            }
        };

        $this.isFunction = function (obj) {
            return $this.type(obj) === 'function';
        };

        $this.notFunction = function (obj) {
            return $this.type(obj) !== 'function';
        };

        $this.isArray = function (obj) {
            return $this.type(obj) === 'array';
        };

        $this.notArray = function (obj) {
            return $this.type(obj) !== 'array';
        };

        $this.isObject = function (obj) {
            // from jQuery
            // Must be an Object.
            // Because of IE, we also have to check the presence of the constructor property.
            // Make sure that DOM nodes and window objects don't pass through, as well
            if (!obj || $this.type(obj) !== "object" || obj.nodeType || $this.isWindow(obj)) {
                return false;
            }

            try {
                // Not own constructor property must be Object
                if (obj.constructor &&
                    !_objProtoHasOwnFunc.call(obj, "constructor") &&
                    !_objProtoHasOwnFunc.call(obj.constructor.prototype, "isPrototypeOf")) {
                    return false;
                }
            } catch (e) {
                // IE8,9 Will throw exceptions on certain host objects #9897
                return false;
            }

            // Own properties are enumerated firstly, so to speed up,
            // if last one is own, then all properties are own.

            var key;
            for (key in obj) { }

            return key === undefined || _objProtoHasOwnFunc.call(obj, key);
        };

        $this.isPlainObject = $this.isObject;

        $this.notObject = function (obj) {
            return $this.isObject(obj) === false;
        };

        $this.isEmptyObject = function(obj) {
            var name;
            for ( name in obj ) {
                return false;
            }
            return true;
        };

        $this.isString = function (obj) {
            return $this.type(obj) === 'string';
        };

        $this.notString = function (obj) {
            return $this.type(obj) !== 'string';
        };

        $this.isBoolean = function (obj) {
            return $this.type(obj) === 'boolean';
        };

        $this.notBoolean = function (obj) {
            return $this.type(obj) !== 'boolean';
        };

        $this.notNullOrWhitespace = function (str) {
            if(!str)
                return false;

            if($this.notString(str))
                throw new Error('Unable to check if a non-string is whitespace.');
            
            // ([^\s]*) = is not whitespace
            // /^$|\s+/ = is empty or whitespace

            return /([^\s])/.test(str);
        };

        $this.isNullOrWhitespace = function (str) {
            return $this.notNullOrWhitespace(str) == false;
        };

        return $this;
    })();

    exceptions = (function (utils) {
        var $this = {};

        $this.exception = function (name, message, data) {
            var _message = utils.isString(message) ? message : name;

            var _err = new Error(_message);
            _err.message = _message;

            if (name != _message)
                _err.name = name;

            if (data)
                _err.data = data;
            return _err;
        };

        $this.argumentException = function (message, argument, data) {
            var _message = utils.notDefined(argument) ? message : message + ' (argument: ' + argument + ')';
            return $this.exception('ArgumentException', _message, data);
        };

        $this.notImplementedException = function (message, data) {
            return $this.exception('NotImplementedException', message, data);
        };

        return $this;
    })(utils);

    models = (function (utils) {
        var $this = {};

        $this.config = function(options) {
            var _self = {}
            options = options || {};

            _self.context = options.name || '_';
            _self.bundleUrl = options.bundleUrl ? utils.enforceLeadingSlash(utils.enforceTrailingSlash(options.bundleUrl)) : '/bundles/';
            _self.debug = options.debug || false;
            _self.baseUrl = options.baseUrl || '/';
            _self.urlArgs = options.urlArgs || null;
            _self.appendTo = options.appendScriptsTo || 'head';
            _self.async = options.async || false;

            return _self;
        };

        $this.bundleVw = function (data) {
            var _self = {};

            _self.name = data.name;
            _self.filePathArray = data.filePathArray;
            _self.resolvedFilePathArray = [];   // for debug, we put each script in the DOM separately. this is for that.
            _self.url = data.url;
            _self.isLoaded = false;
            _self.type = data.type;

            return _self;
        }

        return $this;
    })(utils);

    contexts = {};
    
    var $sixpack = function (options) {
        var $this = {},
            bundles = {},
            defineBundleForDebug,
            loadBundle,
            appendDom,
            firstScript,
            pendingScripts,
            stateChange,
            appendScriptToDom,
            appendStyleToDom,
            context,
            config,
            makeArgument,
            firstArgumentDelim = '?',
            argumentDelim = '&',
            formatUrlArgs,
            baseUrlParam = 'baseUrl',
            urlParam = 'urls';
        
        // get config
        config = models.config(options);

        if(contexts[config.context])
            throw exceptions.argumentException('A sixpack instance with the name, ' + options.name + ', already exists. Sixpack instances must have unique names. Did you forget to pass a name into the sixpack constructor with the options?', 'options.name');

        contexts[config.context] = this;

        formatUrlArgs = function (wantedFirstChar, notWantedFirstChar) {
            var _firstChar = config.urlArgs.substring(0, 1);

            if (_firstChar === notWantedFirstChar)
                config.urlArgs = config.urlArgs.substring(1);

            config.urlArgs = _firstChar === wantedFirstChar ? config.urlArgs : wantedFirstChar + config.urlArgs;
            // TODO: add a regular expression to validate the argument pattern
        };

        // format the urlArgs
        if (config.urlArgs && !config.debug) {
            formatUrlArgs(argumentDelim, firstArgumentDelim);
        }

        // format the urlArgs
        if (config.urlArgs && config.debug) {
            formatUrlArgs(firstArgumentDelim, argumentDelim);
        }

        makeArgument = function (argument, value, isFirst) {
            var _result = isFirst ? firstArgumentDelim : argumentDelim;
            _result += argument + '=' + value;

            return _result;
        };

        // adds a bundle to the context, but does not load it
        $this.defineBundle = function (name, filePathArray) {
            if (bundles[name])
                console.log('WARNING: the ' + name + ' bundle was overwritten.');

            if (utils.isNullOrWhitespace(name))
                throw exceptions.argumentException('all bundles must be given a name', 'name');

            if(utils.notArray(filePathArray))
                throw exceptions.argumentException('the filePathArray must be an array', 'filePathArray');

            if (filePathArray.length < 1)
                throw exceptions.argumentException('at least one path must be included in your bundle', 'filePathArray');

            var _bundle = new models.bundleVw({ 
                name: name, 
                filePathArray: filePathArray, 
                type: filePathArray[0].substring(filePathArray[0].lastIndexOf('.' + 1))
            });

            _bundle.url = config.bundleUrl + name;
            _bundle.url += makeArgument(urlParam, utils.arrayToCommaString(filePathArray), true /* is first argument */);
            _bundle.url += makeArgument(baseUrlParam, config.baseUrl);

            if (config.urlArgs)
                _bundle.url += config.urlArgs;

            if (config.debug)
                return defineBundleForDebug(_bundle);
            return bundles[name] = _bundle;
        }

        defineBundleForDebug = function (bundle) {

            for (var i in bundle.filePathArray) {
                var _preUrl = bundle.filePathArray[i];
                var _url = _preUrl.indexOf('http') >= 0 ? _preUrl : config.baseUrl + utils.removeLeadingSlash(_preUrl);

                if (config.urlArgs)
                    _url += config.urlArgs;

                bundle.resolvedFilePathArray.push(_url);
            }

            return bundles[bundle.name] = bundle;
        }

        $this.load = function (bundleNames) {
            if (utils.isArray(bundleNames)) {
                for (var i in bundleNames) {
                    loadBundle(bundles[bundleNames[i]]);
                }
            }
            else if (utils.notNullOrWhitespace(bundleNames)) {
                loadBundle(bundles[bundleNames]);
            }
        };

        loadBundle = function (bundle) {
            if (bundle.isLoaded)
                return;

            bundle.isLoaded = true;

            if (config.debug) {
                for (var i in bundle.resolvedFilePathArray) {
                    appendDom(bundle.resolvedFilePathArray[i], bundle);
                }
            }
            else {
                appendDom(bundle.url, bundle);
            }
        };

        appendDom = function (src, bundle) {
            switch(bundle.type) {
                case 'js':
                    return appendScriptToDom(src, bundle);
                case 'css':
                    return appendStyleToDom(src, bundle);
                case 'less':
                    return appendStyleToDom(src, bundle);
                case 'sass':
                    return appendStyleToDom(src, bundle);                
                default:
                    return appendScriptToDom(src, bundle);
            };
        };

        // DOM appending concept from http://www.html5rocks.com/en/tutorials/speed/script-loading
        firstScript = document.scripts[0];
        pendingScripts = [];

        // Watch scripts load in IE
        stateChange = function () {
            // Execute as many scripts in order as we can
            var pendingScript;
            while (pendingScripts[0] && pendingScripts[0].readyState == 'loaded') {
                pendingScript = pendingScripts.shift();
                // avoid future loading events from this script (eg, if src changes)
                pendingScript.onreadystatechange = null;
                // can't just appendChild, old IE bug if element isn't closed
                firstScript.parentNode.insertBefore(pendingScript, firstScript);
            }
        }

        appendScriptToDom = function (src, bundle) {
            if ('async' in firstScript) { // modern browsers
                var _script = document.createElement('script');
                _script.async = false;
                _script.src = src;
                _script.setAttribute('data-sixpack-context', config.context);
                _script.setAttribute('data-sixpack-bundle', bundle.name);
                document.head.appendChild(_script);
                document.getElementsByTagName(config.appendTo)[0].appendChild(_script);
            }
            else if (firstScript.readyState) { // IE<10
                // create a script and add it to our todo pile
                var _script = document.createElement('script');
                _script.setAttribute('data-sixpack-context', config.context);
                _script.setAttribute('data-sixpack-bundle', bundle.name);
                pendingScripts.push(_script);
                // listen for state changes
                _script.onreadystatechange = stateChange;
                // must set src AFTER adding onreadystatechange listener
                // else we’ll miss the loaded event for cached scripts
                _script.src = src;
            }
            else { // fall back to defer
                document.write('<script src="' + src
                    + '" data-sixpack-context="' + config.context
                    + '" data-sixpack-bundle="' + bundle.name
                    + '" defer></script>');
            }
        }

        appendStyleToDom = function (src, bundle) {
            throw exceptions.notImplementedException('appendStyleToDom is not yet implemented');
        };

        $this.createContext = this.constructor;
        $this.getBundles = function () { return bundles; };
        $this.getBundle = function (name) { return bundles[name]; };

        return $this;
    }

    return new $sixpack();
})();