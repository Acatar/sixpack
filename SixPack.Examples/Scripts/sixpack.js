var sixpack = (function () {
    var $this = {},
        bundles = [];

    $this.config = {
        context: null,
        bundleUrl: '/bundles/',
        debug: false,
        baseUrl: null,
        urlArgs: null,
        appendTo: 'head',
        async: false
    };

    var bundleVw = function (data) {
        var _self = {};
        _self.name = data.name;
        _self.context = data.context;
        //_self.paths = data.paths; // for require

        return _self;
    }

    $this.defineBundle = function (name, filePathArray, callback)
    {
        var _context = $this.config.context != null ? $this.config.context : '_';

        for (var i in bundles) {     // don't allow the bundle to be loaded more than once per context on a page
            var _lod = bundles[i];
            if (_lod.name == name && _lod.context == _context)
                return;
        }

        if ($this.config.debug)
            return defineBundleForDebug(_context, name, filePathArray, callback);

        var _bundleUrl = $this.config.bundleUrl != null
            ? enforceLeadingSlash(enforceTrailingSlash($this.config.bundleUrl))
            : '/bundles/';
        var _url = _bundleUrl + name;
        _url += '?urls=' + arrayToCommaString(filePathArray);

        if ($this.config.baseUrl)
            _url += '&baseUrl=' + $this.config.baseUrl;

        if ($this.config.urlArgs)
            _url += $this.config.urlArgs.substring(0, 1) == '&' ? $this.config.urlArgs : '&' + $this.config.urlArgs;
        
        appendDom(_url, _context, name);
        bundles.push(new bundleVw({ context: _context, name: name }));
        // TODO: return something that can be used
    }

    var defineBundleForDebug = function (context, name, filePathArray, callback)
    {
        var _filePathArray = [];
        var _baseUrl = $this.config.baseUrl != null ? enforceLeadingSlash(enforceTrailingSlash($this.config.baseUrl)) : '/';

        for (var i in filePathArray) {
            _filePathArray.push(_baseUrl + removeLeadingSlash(filePathArray[i]));
        }

        for (var i in _filePathArray) {
            appendDom(_filePathArray[i], context, name);
        }
        bundles.push(new bundleVw({ context: context, name: name }));
        // TODO: return something that can be used
    }

    var enforceLeadingSlash = function (str) {
        return str.substring(0, 1) == '/' ? str : '/' + str;
    }

    var removeLeadingSlash = function (str) {
        while (str.substring(0, 1) == '/') {
            return removeLeadingSlash(str.substring(1));
        }

        return str;
    }

    var enforceTrailingSlash = function (str) {
        return str.substring(str.length - 1, str.length) == '/' ? str : str + '/';
    }

    var arrayToCommaString = function (array) {
        if (array == null || array.length == 0)
            return null;

        var _output;

        for (var i in array) {
            _output += array[i] + ',';
        }

        return _output.substring(0, _output.length - 1);
    }

    var appendDom = function (src, context, bundleName) {
        var _appendTo = $this.config.appendTo != null && $this.config.appendTo != '' ? $this.config.appendTo : 'head',
            _script = document.createElement('script');

        _script.src = src;
        _script.setAttribute('data-sixpack-context', context);
        _script.setAttribute('data-sixpack-bundle', bundleName);
        if ($this.config.async) {
            _script.async = true;
        }
        else {
            _script.defer = true;
        }

        document.getElementsByTagName(_appendTo)[0].appendChild(_script);
    }

    return $this;
})();