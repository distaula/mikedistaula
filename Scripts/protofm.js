var topNumber = 11;
var chartWidth = 624;
var barHeight = 34;
var barSpacing = barHeight + 1;
var chartHeight = barSpacing * topNumber;

function renderD3(data) {

    var chart = d3.select("#output")
        .append("svg:svg")
        .attr("class", "chart")
        .attr("height", chartHeight)
        .attr("width", chartWidth);

    var maxPlays = d3.max(data, function (d) { return parseInt(d.PlayCount); });
    var minPlays = d3.min(data, function(d) { return parseInt(d.PlayCount); });

    var scale = d3.scale.linear()
        .domain([0, 
                maxPlays])
        .range([0, chartWidth]);
    
    chart.selectAll("rect")
        .data(data)
        .enter().append("svg:rect")
        .attr("y", function(d, i) { return i * barSpacing; })
        .attr("width", function(d) { return scale(d.PlayCount); })
        .attr("height", barHeight);

    chart.selectAll("text")
        .data(data)
        .enter().append("svg:text")
        .attr("x", barHeight + 5)
        .attr("y", function (d, i) { return i * barSpacing; })
        .attr("dx", 10) // padding-right
        .attr("dy", barSpacing/2) // vertical-align: middle
        .attr("text-anchor", "left")
        .attr("fill", "black")
        .text(function (d) { return d.Name + ": " + d.PlayCount; });

    chart.selectAll("image")
        .data(data)
        .enter().append("svg:image")
        .attr("width", barHeight)
        .attr("height", barHeight)
        .attr("xlink:href", function (d) { return d.SmallImage; })
        .attr("preserveAspectRatio", "none")
        .attr("y", function (d, i) { return i * barSpacing; });
}
function renderCircles(json) {

    var vis = d3.select("#output").append("svg:svg")
     .attr("width", w)
     .attr("height", h)
     .attr("class", "pack")
   .append("svg:g")
     .attr("transform", "translate(2, 2)");

        var node = vis.data([json.topartists]).selectAll("g.artist")
            .data(pack.nodes)
            .enter().append("svg:g")
            .attr("class", function (d) {
                return !d.artist ? "top" : "artist";
            })
            .attr("transform", function (d) {
                return "translate(" + d.x + "," + d.y + ")";
            });

        node.append("svg:title")
            .text(function (d) {
                return !d.artist ? "Top Artists" : d.name + ": " + format(d.playcount);
            });

        node.append("svg:circle")
            .attr("r", function (d) {
                return d.r;
            });

        node.filter(function (d) { return !d.artist; }).append("svg:text")
            .attr("text-anchor", "middle")
            .attr("dy", ".3em")
            .text(function (d) { return d.name; });
    }
